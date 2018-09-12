using EjemplosFormacion.HelperClasess.ExtensionMethods;
using EjemplosFormacion.HelperClasess.Office.Abstract;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace EjemplosFormacion.HelperClasess.Office
{
    public class ConstruirTableExcel : IConstruirTableExcel
    {

        #region Metodos Contrato
        public byte[] ConstruirTableExcelFrom<T>(List<T> listaParaExportar, string nombreHoja, bool modoPropiedadInclusivas, params Expression<Func<T, object>>[] propertiesToExport) where T : class
        {
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                List<PropertyInfo> listPropertyInfo = ObtenerPropiedadesAExportar(modoPropiedadInclusivas, propertiesToExport);
                ExcelWorksheet workSheet = excelPackage.Workbook.Worksheets.Add(nombreHoja);

                ExcelTable excelTable = CrearExcelTable(workSheet, listaParaExportar, listPropertyInfo);

                EscribirEncabezados(excelTable, listPropertyInfo);

                EscribirValores(listaParaExportar, workSheet, listPropertyInfo);

                AutoFitTodasLasColumnas(workSheet);

                return excelPackage.GetAsByteArray();
            }
        }
        #endregion

        #region Funciones Privadas
        private static List<PropertyInfo> ObtenerPropiedadesAExportar<T>(bool modoPropiedadInclusivas, Expression<Func<T, object>>[] propertiesToExport) where T : class
        {
            List<string> namesOfPropertiesToExport = propertiesToExport.Select(r => r.GetPropertyName()).ToList();

            Type typeOfObject = typeof(T);
            List<PropertyInfo> listPropertyInfo = typeOfObject.GetProperties().Where(x => namesOfPropertiesToExport.Contains(x.Name) == modoPropiedadInclusivas).ToList();
            return listPropertyInfo;
        }

        private static ExcelTable CrearExcelTable<T>(ExcelWorksheet workSheet, List<T> listaParaExportar, List<PropertyInfo> listPropertyInfo) where T : class
        {
            int numeroFilas = listaParaExportar.Count + 1;
            int numeroColumnas = listPropertyInfo.Count;
            ExcelAddress excelTableAddress = workSheet.Cells[1, 1, numeroFilas, numeroColumnas];

            ExcelTable excelTable = workSheet.Tables.Add(excelTableAddress, "Tabla");
            excelTable.ShowFilter = true;
            excelTable.ShowHeader = true;

            return excelTable;
        }

        private static void EscribirEncabezados(ExcelTable excelTable, List<PropertyInfo> listPropertyInfo)
        {
            int contadorColumna = 0;
            foreach (PropertyInfo propertyInfo in listPropertyInfo)
            {
                excelTable.Columns[contadorColumna].Name = propertyInfo.Name;
                contadorColumna++;
            }
        }

        private static void EscribirValores<T>(List<T> listaParaExportar, ExcelWorksheet workSheet, List<PropertyInfo> listPropertyInfo)
        {
            int contadorFila = 2;
            foreach (T itemParaExportar in listaParaExportar)
            {
                int contadorColumna = 1;
                foreach (var propertyInfo in listPropertyInfo)
                {
                    string value = propertyInfo.GetValue(itemParaExportar)?.ToString();
                    // La libreria asigna el formato de la celda segun el tipo de la variable asignada
                    // Si lo dejas como string, el numero salda como texto en la celda y no se aplicaran formulas
                    bool isNumber = decimal.TryParse(value, out decimal decimalValue);
                    if (isNumber)
                    {
                        workSheet.Cells[contadorFila, contadorColumna].Value = decimalValue;
                        workSheet.Cells[contadorFila, contadorColumna].Style.Numberformat.Format = "0.000";
                    }
                    else
                    {
                        workSheet.Cells[contadorFila, contadorColumna].Value = value;
                    }

                    contadorColumna++;
                }

                contadorFila++;
            }
        }

        protected static void AutoFitTodasLasColumnas(ExcelWorksheet workSheet)
        {
            workSheet.Cells[workSheet.Dimension.Address].AutoFitColumns();
        }
        #endregion

    }
}
