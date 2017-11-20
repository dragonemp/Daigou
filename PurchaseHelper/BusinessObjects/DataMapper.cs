using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Xml;
using System.Collections;

namespace PurchaseHelper.BusinessObjects
{
	/// <summary>
	/// Attribute that identifies the DataBase Field that a property should be mapped to.
	/// Can only be assigned to a Property.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, Inherited=true, AllowMultiple=false)]
	public class DataMapperAttribute : Attribute
	{
		private string _fieldName;
		/// <summary>
		/// The name of the field in the database that the property maps to.
		/// </summary>
		public string FieldName
		{
			get { return _fieldName; }
		}
		/// <summary>
		/// Constructor to set the FieldName property
		/// </summary>
		/// <param name="fieldName"></param>
		public DataMapperAttribute(string fieldName)
		{
			_fieldName = fieldName;
		}
	}
	/// <summary>
	/// Attribute that identifies the DataBase Table that an IEnumerable property should be mapped to.
	/// Can only be assigned to a Property.
	/// </summary>
	[AttributeUsage( AttributeTargets.Property, Inherited = true, AllowMultiple = false )]
	public class DataTableMapperAttribute : Attribute
	{
		public string TableName { get; set; }
		public DataTableMapperAttribute(string tableName)
		{
			TableName = tableName;
		}
	}
	/// <summary>
	/// Attribute that set a User Friendly caption for the property and a resource name for localization of the caption.
	/// </summary>
	[AttributeUsage( AttributeTargets.Property, Inherited = true, AllowMultiple = false )]
	public class DataCaptionAttribute : Attribute
	{
		public string Caption { get; set; }
		public string CaptionResourceName { get; set; }
		public DataCaptionAttribute( string caption )
		{
			Caption = caption;
		}
		public DataCaptionAttribute( string caption, string resourceName )
		{
			Caption = caption;
			CaptionResourceName = resourceName;
		}
	}
	/// <summary>
	/// Class that stores a default caption and resource name for localization. Also can return the localized value for the caption.
	/// </summary>
	public class UICaption
	{
		public string Caption { get; set; }
		public string CaptionResourceName { get; set; }

		public UICaption( string caption )
		{
			Caption = caption;
		}
		public UICaption( string caption, string resourceName )
		{
			Caption = caption;
			CaptionResourceName = resourceName;
		}
		public string GetLocalizedCaption()
		{
            return Caption;
		}
	}
	public class ContractWrapper<T> where T : new()
	{
		private T contract;
		public T DataContract
		{
			get 
			{
				if ( contract == null ) contract = new T();
				return contract; 
			}
		}
		private Dictionary<string, PropertyInfo> fieldMap;
		public Dictionary<string, PropertyInfo> FieldMap
		{
			get
			{
				if ( fieldMap == null ) fieldMap = new Dictionary<string, PropertyInfo>();
				return fieldMap;
			}
		}
		private Dictionary<string, PropertyInfo> childMap;
		public Dictionary<string, PropertyInfo> ChildMap
		{
			get 
			{
				if ( childMap == null ) childMap = new Dictionary<string, PropertyInfo>();
				return childMap; 
			}
		}
		private Dictionary<string, UICaption> fieldCaptions;
		public Dictionary<string, UICaption> FieldCaptions
		{
			get
			{
				if ( fieldCaptions == null ) fieldCaptions = new Dictionary<string, UICaption>();
				return fieldCaptions;
			}
		}

		public object this[string FieldName]
		{
			get
			{
				return GetFieldValue( FieldName );
			}
			set
			{
				SetFieldValue( FieldName, value );
			}
		}
		public ContractWrapper()
		{
			fieldMap = mapPropertiesToFieldNames();
		}
		public ContractWrapper(T Contract)
		{
			contract = Contract;
			fieldMap = mapPropertiesToFieldNames();
		}
		public T GetNewContract()
		{
			contract = new T();
			return contract;
		}
		public object GetFieldValue(string FieldName)
		{
			if ( fieldMap.ContainsKey( FieldName ) )
			{
				return fieldMap[FieldName].GetValue( contract, null );
			}
			return null;
		}
		public void SetFieldValue(string FieldName, object Value)
		{
			if ( fieldMap.ContainsKey( FieldName ) )
			{
				fieldMap[FieldName].SetValue( contract, Value, null );
			}
		}
		public IEnumerable GetChildList(string TableName)
		{
			if ( childMap.ContainsKey( TableName ) )
			{
				object childList = childMap[TableName].GetValue( contract, null );
				return (IEnumerable)childList;
				//return (childList as IEnumerable<IChildContract>);
			}
			return null;
		}
		private Dictionary<string, PropertyInfo> mapPropertiesToFieldNames()
		{
			Dictionary<string, PropertyInfo> map = new Dictionary<string, PropertyInfo>();
			childMap = new Dictionary<string, PropertyInfo>();
			Type t = contract == null ? typeof( T ) : contract.GetType();
			PropertyInfo[] props = t.GetProperties();
			DataMapperAttribute dma;
			DataTableMapperAttribute dtm;
			DataCaptionAttribute dca;
			for ( int i = 0; i < props.Length; i++ )
			{
				dma = (DataMapperAttribute)Attribute.GetCustomAttribute( props[i], typeof( DataMapperAttribute ) );
				if ( dma != null )
				{
					map[dma.FieldName] = props[i];
				}
				else
				{
					dtm = (DataTableMapperAttribute)Attribute.GetCustomAttribute( props[i], typeof( DataTableMapperAttribute ) );
					if ( dtm != null && props[i].PropertyType.GetInterface( "System.Collections.IEnumerable" ) != null )
					{
						childMap[dtm.TableName] = props[i];
					}
				}
				dca = (DataCaptionAttribute)Attribute.GetCustomAttribute( props[i], typeof( DataCaptionAttribute ) );
				if ( dca != null )
				{
					FieldCaptions.Add( props[i].Name, new UICaption( dca.Caption, dca.CaptionResourceName ) );
				}
			}
			return map;
		}
	}
	/// <summary>
	/// Base class for data mappers that can map a data source to an entities properties
	/// and hydrate the entity with values.
	/// The mapping is accomplished by using the DataMapperAttribute assigned to the property.
	/// </summary>
	/// <typeparam name="TSource">A Business Entity type</typeparam>
	public abstract class BaseMapper<TSource>
	{
		/// <summary>
		/// Method to call hydrate an entity from the data source.
		/// </summary>
		/// <typeparam name="TEntity">Business Entity type.</typeparam>
		/// <param name="DataSource">The source of values for the Entity.</param>
		/// <param name="Entity">The Entity to hydrate with values.</param>
		/// <returns>The hydrated Entity.</returns>
		public virtual TEntity MapDataToFields<TEntity>(TSource DataSource, TEntity Entity)
		{
			mapToFields<TEntity>( DataSource, ref Entity );
			return Entity;
		}
		public virtual TEntity MapDataToFields<TEntity>(TSource DataSource, ContractWrapper<TEntity> Contract) where TEntity : new()
		{
			return mapToFields<TEntity>( DataSource, Contract );
		}
		/// <summary>
		/// Retrieves a value from the data source.
		/// </summary>
		/// <param name="src"></param>
		/// <param name="FieldName"></param>
		/// <param name="Value"></param>
		/// <returns></returns>
		public bool GetValue(TSource src, string FieldName, ref object Value)
		{
			return getValue( src, FieldName, ref Value );
		}
		/// <summary>
		/// Implementation of the mapping functionality using the DataMapperAttribute. Uses reflection
		/// to interate through the properties of the entity and reads the DataMapperAttribute to determine
		/// which data point to use for the value.
		/// </summary>
		/// <typeparam name="TEntity">Business Entity type to hydrate.</typeparam>
		/// <param name="src">Source of the values.</param>
		/// <param name="entity">Instance of Entity to hydrate.</param>
		protected virtual void mapToFields<TEntity>(TSource src, ref TEntity entity)
		{
			Type t = entity.GetType();
			PropertyInfo[] props = t.GetProperties();
			DataMapperAttribute dma ;
			for ( int i = 0; i < props.Length; i++ )
			{
				dma = (DataMapperAttribute)Attribute.GetCustomAttribute( props[i], typeof( DataMapperAttribute ) );
				if ( dma != null )
				{
					object value = null; 
					if ( getValue( src, dma.FieldName, ref value ) )
					{
						try
						{
							value = convertValue(props[i], value);
							if ( value != null )
							{
								props[i].SetValue( entity, value, null );
							}
						}
						catch ( Exception ex )
						{
							throw new Exception( "Error setting property for " + dma.FieldName, ex );
						}
					}
				}
			}
		}
		protected virtual TEntity mapToFields<TEntity>(TSource src, ContractWrapper<TEntity> contract) where TEntity : new()
		{
			TEntity entity = contract.GetNewContract();
			foreach ( KeyValuePair<string, PropertyInfo> kvp in contract.FieldMap )
			{
				object value = null;
				if ( getValue( src, kvp.Key, ref value ) )
				{
					try
					{
						value = convertValue(kvp.Value, value);
						if ( value != null )
						{
							contract.SetFieldValue(kvp.Key, value);
						}
					}
					catch ( Exception ex )
					{
						throw new Exception( "Error setting property for " + kvp.Key, ex );
					}
				}
			}
			return entity;
		}

		/// <summary>
		/// Converts value to correct type based on property
		/// </summary>
		/// <param name="prop">Property</param>
		/// <param name="value">Value</param>
		/// <returns></returns>
		private object convertValue(PropertyInfo prop, object value)
		{
			// Handle booleans
			if ( ( prop.PropertyType == typeof(bool) ) || ( prop.PropertyType == typeof(bool?) ) )
			{
				if ( value == null )
				{
					value = false;
				}
				else
				{
					value = ( value.ToString() == "-1" );
				}
			}

			// Handle doubles/decimals
			if ( value != null && ( value.GetType() == typeof(System.Decimal) || value.GetType() == typeof(System.Double) ) )
			{
				if ( ( prop.PropertyType == typeof(double) ) || ( prop.PropertyType == typeof(double?) ) )
				{
					value = Convert.ToDouble(value);
				}
				else
				{
					value = Convert.ToInt32(value);
				}
			}

			// Handle enums
			Type enumType = null;
			if ( value != null && prop.PropertyType.BaseType == typeof(Enum) )
			{
				enumType = prop.PropertyType;
			}

			// Handle nullable enums
			if ( value != null && prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)
				&& prop.PropertyType.GetGenericArguments()[0].BaseType == typeof(Enum) )
			{
				enumType = prop.PropertyType.GetGenericArguments()[0];
			}

			if ( enumType != null )
			{
				MethodInfo castMethod = this.GetType().GetMethod("Cast").MakeGenericMethod(enumType);
				value = castMethod.Invoke(this, new object[] { value });
			}

			return value;
		}
		/// <summary>
		/// Generic cast method, used for casting int to enum with reflected types
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="o"></param>
		/// <returns></returns>
		public T Cast<T>(object o)
		{
			return (T)o;
		}
		/// <summary>
		/// Place holder for method to get the value from the source. Must be implemented in a
		/// concrete class based on this class.
		/// </summary>
		/// <param name="src">Source of the values.</param>
		/// <param name="fieldName">Name of the field or data point that contains the value to retrieve.</param>
		/// <param name="value">Reference to the value to be retrieved.</param>
		/// <returns>True if the source contains a value for the fieldName, otherwise, false.</returns>
		protected abstract bool getValue(TSource src, string fieldName, ref object value);
		public Dictionary<string, object> ContractToDictionary<T>(T DataContract) where T : new()
		{
			Dictionary<string, object> d = new Dictionary<string, object>();

			ContractWrapper<T> cw = new ContractWrapper<T>( DataContract );
			foreach ( string key in cw.FieldMap.Keys )
			{
				d.Add( key, cw[key] );
			}
			return d;
		}
	}
	/// <summary>
	/// Concrete Mapper class that hydrates a Business Entity from a DataRow
	/// </summary>
	public class DataRowMapper : BaseMapper<DataRow>
	{
		/// <summary>
		/// Implementation of the base class's method used to get the value from the DataRow.
		/// </summary>
		/// <param name="row">DataRow that contains the values for the Business Entity.</param>
		/// <param name="fieldName">Name of the field that maps to the current Property.</param>
		/// <param name="value">Reference to the value to be retrieved.</param>
		/// <returns>True if the DataRow has a column for fieldName, otherwise, false.</returns>
		protected override bool getValue(DataRow row, string fieldName, ref object value)
		{
			bool valueFound = false;
			if ( row.Table.Columns.Contains( fieldName ) )
			{
				valueFound = true;
				try
				{
					value = Convert.IsDBNull( row[fieldName] ) ? null : row[fieldName];
				}
				catch ( Exception ex )
				{
					throw new Exception( "Error getting value from DataRow for " + fieldName, ex );
				}
			}
			return valueFound;
		}
	}
	/// <summary>
	/// Concrete Mapper class that hydrates a Business Entity from a DataReader
	/// </summary>
	public class DataReaderMapper : BaseMapper<DbDataReader>
	{
		private List<string> columnNames = null;
		/// <summary>
		/// Implementation of the base class's method used to get the value from the DataReader.
		/// </summary>
		/// <param name="src">DataReader that contains the value. Must be on the correct record already.</param>
		/// <param name="fieldName">Name of the field that maps to the current Property.</param>
		/// <param name="value">Reference to the value to be retrieved.</param>
		/// <returns>True if the DataReader has a column for fieldName, otherwise, false.</returns>
		protected override bool getValue(DbDataReader src, string fieldName, ref object value)
		{
			bool valueFound = true;
			int index = 0;
			
			// Cache list of column names in data reader
			if (columnNames == null)
			{
				columnNames = new List<string>();
				for (var i = 0; i < src.FieldCount; i++)
				{
					columnNames.Add(src.GetName(i).ToUpperInvariant().Trim());
				}
			}
			// Check field name against list of column names to improve performance (by about 10x)
			if (columnNames.Contains(fieldName.ToUpperInvariant().Trim()))
			{
				try
				{
					index = src.GetOrdinal(fieldName);
					value = src.IsDBNull(index) ? null : src.GetValue(index);
				}
				catch (IndexOutOfRangeException error) // should not get hit anymore with columnNames check above
				{
					valueFound = false;
				}
				catch (Exception ex)
				{
					throw new Exception("Error getting value from DataReader for " + fieldName, ex);
				}
				return valueFound;
			}
			else return false;
		}
	}
	/// <summary>
	///  Concrete Mapper class that hydrates a Business Entity from an XmlNode
	/// </summary>
	public class XmlNodeMapper : BaseMapper<XmlNode>
	{
		/// <summary>
		/// Implementation of the base class's method used to get the value from the XmlNode.
		/// </summary>
		/// <param name="node">XmlNode that contains the values for the Business Entity.</param>
		/// <param name="fieldName">Name of the Node that maps to the current Property.</param>
		/// <param name="value">Reference to the value to be retrieved.</param>
		/// <returns>True if the XmlNode has a child node named fieldName, otherwise, false.</returns>
		protected override bool getValue(XmlNode node, string fieldName, ref object value)
		{
			bool valueFound = false;
			if ( node.SelectNodes( fieldName ).Count > 0 )
			{
				valueFound = true;
				try
				{
					XmlNode valueNode = node.SelectSingleNode( fieldName );
					value = valueNode.InnerText == "*NULL*" ? null : valueNode.InnerText;
				}
				catch ( Exception ex )
				{
					throw new Exception( "Error getting value from XML Node for " + fieldName, ex );
				}
			}
			return valueFound;
		}
	}
	///// <summary>
	///// Example Entity class that can be hydrated using Data Mappers.
	///// </summary>
	//public class ProductContract
	//{
	//    private int sys_product_id;
	//    private string sys_product_name;
	//    private int? prod_ord_num;
	//    private string targeturl;
	//    private string imageurl;
	//    /// <summary>
	//    /// Unique integer ID for the Product.
	//    /// </summary>
	//    [DataMapper( "SYS_PRODUCT_ID" )]
	//    public int ProductID
	//    {
	//        get { return sys_product_id; }
	//        set { sys_product_id = value; }
	//    }
	//    /// <summary>
	//    /// Name of the Product
	//    /// </summary>
	//    [DataMapper( "SYS_PRODUCT_NAME" )]
	//    public string ProductName
	//    {
	//        get { return sys_product_name; }
	//        set { sys_product_name = value; }
	//    }
	//    /// <summary>
	//    /// Integer used to controls the display order of the Products.
	//    /// </summary>
	//    [DataMapper( "PROD_ORD_NUMS" )]
	//    public int? ProductOrder
	//    {
	//        get { return prod_ord_num; }
	//        set { prod_ord_num = value; }
	//    }
	//    /// <summary>
	//    /// URL of the page to open when Product is selected.
	//    /// </summary>
	//    [DataMapper( "TARGETURL" )]
	//    public string TargetURL
	//    {
	//        get { return targeturl; }
	//        set { targeturl = value; }
	//    }
	//    /// <summary>
	//    /// URL of an image associated with the Product.
	//    /// </summary>
	//    [DataMapper( "IMAGEURL" )]
	//    public string ImageURL
	//    {
	//        get { return imageurl; }
	//        set { imageurl = value; }
	//    }
	//    #region IESuiteEntity Members
	//    /// <summary>
	//    /// Unique integer ID for the Product.
	//    /// </summary>
	//    public int ID
	//    {
	//        get { return (sys_product_id); }
	//    }
	//    /// <summary>
	//    /// Name of the Product
	//    /// </summary>
	//    public string Name
	//    {
	//        get { return sys_product_name; }
	//    }
	//    #endregion
	//    /// <summary>
	//    /// Empty constructor.
	//    /// </summary>
	//    public ProductContract() { }
	//}
}
