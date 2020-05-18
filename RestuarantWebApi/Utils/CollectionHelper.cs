using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace RestuarantWebApi.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class CollectionHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    T item = GetItem<T>(row);
                    data.Add(item);
                }
            }
            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static T convetDataTableToSingleRecord<T>(DataTable dt)
        {
            T item = default;
            if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
            {
                item = GetItem<T>(dt.Rows[0]);
            }
            return item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                if (dr[column.ColumnName] != DBNull.Value)
                {
                    foreach (PropertyInfo pro in temp.GetProperties())
                    {
                        if (pro.Name.ToUpper() == column.ColumnName.ToUpper())
                        {
                            if (pro.PropertyType == typeof(System.Int32))
                            {
                                int val = Convert.ToInt32(dr[column.ColumnName]);
                                pro.SetValue(obj, val, null);
                            }
                            else if (pro.PropertyType == typeof(System.Char))
                            {
                                char val = Convert.ToChar(dr[column.ColumnName]);
                                pro.SetValue(obj, val, null);
                            }
                            else if (pro.PropertyType == typeof(System.Int64))
                            {
                                Int64 val = Convert.ToInt64(dr[column.ColumnName]);
                                pro.SetValue(obj, val, null);
                            }
                            else if (pro.PropertyType == typeof(System.Int16))
                            {
                                Int16 val = Convert.ToInt16(dr[column.ColumnName]);
                                pro.SetValue(obj, val, null);
                            }
                            else if (pro.PropertyType == typeof(System.Double))
                            {
                                Double val = Convert.ToDouble(dr[column.ColumnName]);
                                pro.SetValue(obj, val, null);
                            }
                            else if (pro.PropertyType == typeof(System.DateTime))
                            {
                                DateTime val = Convert.ToDateTime(dr[column.ColumnName]);
                                pro.SetValue(obj, val, null);
                            }
                            else if (pro.PropertyType == typeof(System.String))
                            {
                                System.String val = Convert.ToString(dr[column.ColumnName]);
                                pro.SetValue(obj, val, null);
                            }
                        }
                        else
                            continue;
                    }
                }
            }
            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(List<T> items)
        {

            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties

            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in Props)

            {

                //Setting column names as Property names

                dataTable.Columns.Add(prop.Name);

            }

            foreach (T item in items)

            {

                var values = new object[Props.Length];

                for (int i = 0; i < Props.Length; i++)

                {
                    //inserting property values to datatable rows

                    values[i] = Props[i].GetValue(item, null);

                }

                dataTable.Rows.Add(values);

            }
            return dataTable;

        }
    }
}
