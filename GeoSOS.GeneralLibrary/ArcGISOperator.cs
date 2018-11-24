using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.DataSourcesRaster;
using GeoSOS.CommonLibrary.Struct;

namespace GeoSOS.CommonLibrary
{
    public class ArcGISOperator
    {
        private static IMap map;

        public static IMap FoucsMap
        {
            set { map = value; }
            get { return map; }
        }

        public static IRasterLayer GetRasterLayerByName(string layerName)
        {
            IRasterLayer rasterLayer = null;
            for (int i = 0; i < map.LayerCount; i++)
            {
                if (map.get_Layer(i).Name == layerName)
                    rasterLayer = (IRasterLayer)map.get_Layer(i);
            }
            return rasterLayer;
        }

        /// <summary>
        /// 读取数据并记录非空数据的行列号。
        /// </summary>
        public static float[,] ReadRasterAndGetNotNullRowColumn(IRasterLayer rasterLayer, out StructRasterMetaData structRasterMetaData,
            out List<int> notNullRows, out List<int> notNullColumns)
        {
            int rowCount, columnCount;
            notNullRows = new List<int>();
            notNullColumns = new List<int>();

            structRasterMetaData.ColumnCount = rasterLayer.ColumnCount;
            columnCount = structRasterMetaData.ColumnCount;
            structRasterMetaData.RowCount = rasterLayer.RowCount;
            rowCount = structRasterMetaData.RowCount;

            IEnvelope visiableExtent = rasterLayer.VisibleExtent;
            structRasterMetaData.XMin = visiableExtent.XMin;
            structRasterMetaData.XMax = visiableExtent.XMax;
            structRasterMetaData.YMin = visiableExtent.YMin;
            structRasterMetaData.YMax = visiableExtent.YMax;
            structRasterMetaData.NoDataValue = -9999f;

            IRaster2 raster = rasterLayer.Raster as IRaster2;
            IPnt fromPnt = new PntClass();
            fromPnt.SetCoords(0, 0);
            IPnt blockSize = new PntClass();
            blockSize.SetCoords(columnCount, rowCount);
            IPixelBlock pixelBlock = ((IRaster)raster).CreatePixelBlock(blockSize);
            rasterLayer.Raster.Read(fromPnt, pixelBlock);
            float[,] data = new float[rowCount, columnCount];

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    object value = pixelBlock.GetVal(0, j, i);
                    if (value != null)
                    {
                        data[i, j] = Convert.ToSingle(value);
                        notNullRows.Add(i);
                        notNullColumns.Add(j);
                    }
                    else
                        data[i, j] = structRasterMetaData.NoDataValue;
                }
            }
            return data;
        }

        /// <summary>
        /// 读取栅格数据。
        /// </summary>
        public static float[,] ReadRaster(IRasterLayer rasterLayer, float nullValue)
        {
            int columnCount = rasterLayer.ColumnCount;
            int rowCount = rasterLayer.RowCount;

            IRaster2 raster = rasterLayer.Raster as IRaster2;
            IPnt fromPnt = new PntClass();
            fromPnt.SetCoords(0, 0);
            IPnt blockSize = new PntClass();
            blockSize.SetCoords(columnCount, rowCount);
            IPixelBlock pixedBlock = ((IRaster)raster).CreatePixelBlock(blockSize);
            rasterLayer.Raster.Read(fromPnt, pixedBlock);

            float[,] data = new float[rowCount, columnCount];

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    object value = pixedBlock.GetVal(0, j, i);
                    if (value != null)
                        data[i, j] = Convert.ToSingle(value);
                    else
                        data[i, j] = nullValue;
                }
            }
            return data;
        }

        /// <summary>
        /// 读取栅格数据，并得到图层数据的元数据。
        /// </summary>
        public static float[,] ReadRasterAndGetMetaData(IRasterLayer rasterLayer, out StructRasterMetaData structRasterMetaData)
        {
            int rowCount, columnCount;

            structRasterMetaData.ColumnCount = rasterLayer.ColumnCount;
            columnCount = structRasterMetaData.ColumnCount;
            structRasterMetaData.RowCount = rasterLayer.RowCount;
            rowCount = structRasterMetaData.RowCount;

            IEnvelope visiableExtent = rasterLayer.VisibleExtent;
            structRasterMetaData.XMin = visiableExtent.XMin;
            structRasterMetaData.XMax = visiableExtent.XMax;
            structRasterMetaData.YMin = visiableExtent.YMin;
            structRasterMetaData.YMax = visiableExtent.YMax;
            structRasterMetaData.NoDataValue = -9999f;

            IRaster2 raster = rasterLayer.Raster as IRaster2;
            IPnt fromPnt = new PntClass();
            fromPnt.SetCoords(0, 0);
            IPnt blockSize = new PntClass();
            blockSize.SetCoords(columnCount, rowCount);
            IPixelBlock pixedBlock = ((IRaster)raster).CreatePixelBlock(blockSize);
            rasterLayer.Raster.Read(fromPnt, pixedBlock);

            float[,] data = new float[rowCount, columnCount];

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    object value = pixedBlock.GetVal(0, j, i);
                    if (value != null)
                        data[i, j] = Convert.ToSingle(value);
                    else
                        data[i, j] = structRasterMetaData.NoDataValue;
                }
            }
            return data;
        }

        /// <summary>
        /// 写入栅格数据。
        /// </summary>
        public static void WriteRaster(IRasterLayer rasterLayer, float[,] datas)
        {
            int columnCount = rasterLayer.ColumnCount;
            int rowCount = rasterLayer.RowCount;

            IRaster2 raster = rasterLayer.Raster as IRaster2;
            IPnt fromPnt = new PntClass();
            fromPnt.SetCoords(0, 0);
            IPnt blockSize = new PntClass();
            blockSize.SetCoords(columnCount, rowCount);
            IPixelBlock pixelBlock = ((IRaster)raster).CreatePixelBlock(blockSize);
            rasterLayer.Raster.Read(fromPnt, pixelBlock);

            IPixelBlock3 pixelBlock3 = (IPixelBlock3)pixelBlock;
            object temp = pixelBlock3.get_PixelDataByRef(0);
            if ((pixelBlock3.get_PixelType(0) == rstPixelType.PT_CLONG) || (pixelBlock3.get_PixelType(0) == rstPixelType.PT_ULONG))
            {
                System.Int32[,] pixelData = (System.Int32[,])temp;
                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < columnCount; j++)
                    {
                        if (datas[i, j] == -9999f)
                            pixelData[j, i] = 0;
                        else
                        {
                            if (pixelData[j, i] != Convert.ToInt32(datas[i, j]))
                                pixelData[j, i] = Convert.ToInt32(datas[i, j]);
                        }
                    }
                }
                pixelBlock3.set_PixelData(0, pixelData);
            }
            else if ((pixelBlock3.get_PixelType(0) == rstPixelType.PT_SHORT) || (pixelBlock3.get_PixelType(0) == rstPixelType.PT_USHORT))
            {
                System.Int16[,] pixelData = (System.Int16[,])temp;
                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < columnCount; j++)
                    {
                        if (datas[i, j] == -9999f)
                            pixelData[j, i] = 0;
                        else
                        {
                            if (pixelData[j, i] != Convert.ToInt16(datas[i, j]))
                                pixelData[j, i] = Convert.ToInt16(datas[i, j]);
                        }
                    }
                }
                pixelBlock3.set_PixelData(0, pixelData);
            }
            else if ((pixelBlock3.get_PixelType(0) == rstPixelType.PT_CHAR) || (pixelBlock3.get_PixelType(0) == rstPixelType.PT_UCHAR))
            {
                System.Byte[,] pixelData = (System.Byte[,])temp;
                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < columnCount; j++)
                    {
                        if (datas[i, j] == -9999f)
                            //pixelData[j, i] = 0;
                            ;
                        else
                        {
                            if (pixelData[j, i] != Convert.ToByte(datas[i, j]))
                                pixelData[j, i] = Convert.ToByte(datas[i, j]);
                        }
                    }
                }
                pixelBlock3.set_PixelData(0, pixelData);
            }
            else if (pixelBlock3.get_PixelType(0) == rstPixelType.PT_FLOAT)
            {
                System.Single[,] pixelData = (System.Single[,])temp;
                for (int i = 0; i < rowCount; i++)
                {
                    for (int j = 0; j < columnCount; j++)
                    {
                        if (datas[i, j] == -9999f)
                            //pixelData[j, i] =0;
                            ;
                        else
                        {
                            if (pixelData[j, i] != Convert.ToSingle(datas[i, j]))
                                pixelData[j, i] = Convert.ToSingle(datas[i, j]);
                        }
                    }
                }
                pixelBlock3.set_PixelData(0, pixelData);
            }

            ((IRasterEdit)rasterLayer.Raster).Write(fromPnt, (IPixelBlock)pixelBlock3);
            ((IRasterEdit)rasterLayer.Raster).Refresh();
        }

        /// <summary>
        /// 获取当前各数据的最小空间范围。
        /// </summary>
        /// <param name="trainingStartLayer"></param>
        /// <param name="trainingEndLayer"></param>
        /// <param name="variablesLayers"></param>
        /// <param name="structRasterMetaData"></param>
        public static void GetSmallestBound(IRasterLayer trainingStartLayer, IRasterLayer trainingEndLayer, List<IRasterLayer> variablesLayers,
            ref StructRasterMetaData structRasterMetaData)
        {
            int rowCount = structRasterMetaData.RowCount;
            int columnCount = structRasterMetaData.ColumnCount;
            if ((trainingStartLayer.RowCount < rowCount) && (trainingStartLayer.ColumnCount < columnCount))
            {
                rowCount = trainingStartLayer.RowCount;
                columnCount = trainingStartLayer.ColumnCount;
            }
            if ((trainingEndLayer.RowCount < rowCount) && (trainingEndLayer.ColumnCount < columnCount))
            {
                rowCount = trainingEndLayer.RowCount;
                columnCount = trainingEndLayer.ColumnCount;
            }
            for (int i = 0; i < variablesLayers.Count; i++)
            {
                if ((variablesLayers[i].RowCount < rowCount) && (variablesLayers[i].ColumnCount < columnCount))
                {
                    rowCount = variablesLayers[i].RowCount;
                    columnCount = variablesLayers[i].ColumnCount;
                }
            }
            structRasterMetaData.RowCount = rowCount;
            structRasterMetaData.ColumnCount = columnCount;
        }

        /// <summary>
        /// 获取当前栅格图层中城市用地类型栅格的数量。区分栅格图层是否有属性表两种情况。
        /// </summary>
        /// <param name="rasterLayer"></param>
        /// <param name="landUseClassificationInfo"></param>
        /// <returns></returns>
        public static int GetUrbanCount(IRasterLayer rasterLayer, LandUseClassificationInfo landUseClassificationInfo)
        {
            int urbanConuts = 0;
            IRasterBandCollection rbc = rasterLayer.Raster as IRasterBandCollection;
            IRasterBand rb = rbc.Item(0);
            //ITable table = rbc.Item(0).AttributeTable;
            //AttributeTable - OID, Value, Count
            ITable attributeTable = rb.AttributeTable;
            if (attributeTable != null)
            {
                int count = attributeTable.RowCount(null);
                int[] values = new int[count];
                int[] counts = new int[count];
                ICursor cursor = attributeTable.Search(null, false);
                IRow row = cursor.NextRow();
                int index = 0;
                //数据不一定有OBJECTID字段。同时列名称及顺序不一定符合条件Value,Count。
                while (row != null)
                {
                    values[index] = Convert.ToInt32(row.get_Value(GetColumnIndex(attributeTable, "VALUE")));
                    counts[index] = Convert.ToInt32(row.get_Value(GetColumnIndex(attributeTable, "COUNT")));
                    row = cursor.NextRow();
                    index++;
                }

                for (int j = 0; j < count; j++)
                {
                    for (int i = 0; i < landUseClassificationInfo.UrbanValues.Count; i++)
                    {
                        if (Convert.ToSingle(values[j]) == landUseClassificationInfo.UrbanValues[i].LanduseTypeValue)
                            urbanConuts += Convert.ToInt32(counts[j]);
                    }
                }
            }
            else
            {
                rb.ComputeStatsAndHist();
                IRasterHistogram rh = rb.Histogram;
                object rhCount = rh.Counts;
                double[] counts = (double[])rhCount;
                IRasterUniqueValueRenderer rasterUniqueValueRenderer = (IRasterUniqueValueRenderer)rasterLayer.Renderer;
                IRasterRendererUniqueValues rasterRendererUniqueValues = (IRasterRendererUniqueValues)rasterUniqueValueRenderer;
                IUniqueValues uniqueValues = rasterRendererUniqueValues.UniqueValues;
                for (int j = 0; j < uniqueValues.Count; j++)
                {
                    for (int i = 0; i < landUseClassificationInfo.UrbanValues.Count; i++)
                    {
                        if (Convert.ToSingle(uniqueValues.get_UniqueValue(j)) == landUseClassificationInfo.UrbanValues[i].LanduseTypeValue)
                            urbanConuts += Convert.ToInt32(counts[j + 1]);
                    }
                }
            }

            return urbanConuts;
        }

        /// <summary>
        /// 获取当前栅格图层中各种用地类型栅格的数量。区分栅格图层是否有属性表两种情况。
        /// </summary>
        /// <param name="rasterLayer"></param>
        /// <returns></returns>
        public static int[] GetLandUseTypesCount(IRasterLayer rasterLayer)
        {
            int[] landusetypesCounts;
            IRasterBandCollection rbc = rasterLayer.Raster as IRasterBandCollection;
            IRasterBand rb = rbc.Item(0);
            ITable attributeTable = rb.AttributeTable;
            if (attributeTable != null)
            {
                int count = attributeTable.RowCount(null);
                int[] values = new int[count];
                landusetypesCounts = new int[count];
                ICursor cursor = attributeTable.Search(null, false);
                IRow row = cursor.NextRow();
                int index = 0;
                //数据不一定有OBJECTID字段。同时列名称及顺序不一定符合条件Value,Count。
                while (row != null)
                {
                    values[index] = Convert.ToInt32(row.get_Value(GetColumnIndex(attributeTable, "VALUE")));
                    landusetypesCounts[index] = Convert.ToInt32(row.get_Value(GetColumnIndex(attributeTable, "COUNT")));
                    row = cursor.NextRow();
                    index++;
                }
            }
            else
            {
                rb.ComputeStatsAndHist();
                IRasterHistogram rh = rb.Histogram;
                object rhCount = rh.Counts;
                double[] counts = (double[])rhCount;
                landusetypesCounts = new int[counts.Length];
                IRasterUniqueValueRenderer rasterUniqueValueRenderer = (IRasterUniqueValueRenderer)rasterLayer.Renderer;
                IRasterRendererUniqueValues rasterRendererUniqueValues = (IRasterRendererUniqueValues)rasterUniqueValueRenderer;
                IUniqueValues uniqueValues = rasterRendererUniqueValues.UniqueValues;
                for (int j = 0; j < uniqueValues.Count; j++)
                    landusetypesCounts[j] = Convert.ToInt32(counts[j]);
            }
            return landusetypesCounts;
        }

        /// <summary>
        /// 获取表格中某列的索引。
        /// </summary>
        /// <param name="table"></param>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public static int GetColumnIndex(ITable table, string columnName)
        {
            int index = -1;
            for (int i = 0; i < table.Fields.FieldCount; i++)
            {
                if (table.Fields.Field[i].Name.ToUpper() == columnName)
                    index = i;
            }
            return index;
        }

        public static IRasterDataset CreateRasterDataset(string filePath, string fileName, IRasterLayer rasterLayer,
            StructRasterMetaData structRasterMetaData, int[,] data, int noDataValue)
        {
            try
            {
                IRasterWorkspace2 rasterWorkspace2 = OpenRasterWorkspace(filePath);
                //Define the origin for the raster dataset, which is the lower left corner of the raster.
                IPoint originPoint = new PointClass();
                originPoint.PutCoords(structRasterMetaData.XMin, structRasterMetaData.YMin);
                //Define the dimensions of the raster dataset.
                int width = structRasterMetaData.RowCount; //This is the width of the raster dataset.
                int height = structRasterMetaData.ColumnCount; //This is the height of the raster dataset.
                IRaster r = rasterLayer.Raster;
                IRasterDefaultProps rdp = r as IRasterDefaultProps;
                double xCellSize = rdp.DefaultPixelWidth; //This is the cell size in x direction.
                double yCellSize = rdp.DefaultPixelHeight; //This is the cell size in y direction.
                ISpatialReference spatialReference = rdp.DefaultSpatialReference;
                int bandCount = 1; // This is the number of bands the raster dataset contains.
                //Create a raster dataset in TIFF format.
                IRasterDataset rasterDataset = rasterWorkspace2.CreateRasterDataset(fileName, "IMAGINE Image",
                    originPoint, height, width, xCellSize, yCellSize, bandCount, rstPixelType.PT_UCHAR, spatialReference,
                    true);

                //If you need to set NoData for some of the pixels, you need to set it on band 
                //to get the raster band.
                IRasterBandCollection rasterBands = (IRasterBandCollection)rasterDataset;
                IRasterBand rasterBand;
                IRasterProps rasterProps;
                rasterBand = rasterBands.Item(0);
                rasterProps = (IRasterProps)rasterBand;
                //Set NoData if necessary. For a multiband image, a NoData value needs to be set for each band.
                rasterProps.NoDataValue = -9999f;
                //Create a raster from the dataset.
                IRaster raster = rasterDataset.CreateDefaultRaster();

                //Create a pixel block using the weight and height of the raster dataset. 
                //If the raster dataset is large, a smaller pixel block should be used. 
                //Refer to the topic "How to access pixel data using a raster cursor".
                IPnt blocksize = new PntClass();
                blocksize.SetCoords(height, width);
                IPixelBlock3 pixelblock = raster.CreatePixelBlock(blocksize) as IPixelBlock3;

                object temp = pixelblock.get_PixelDataByRef(0);

                System.Byte[,] pixelData = (System.Byte[,])temp;
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        if (data[i, j] == -9999f)
                        {
                            pixelData[j, i] = (System.Byte)noDataValue;
                            //System.Diagnostics.Debug.WriteLine(i.ToString() + "+" + j.ToString());
                        }
                        else
                        {
                            if (pixelData[j, i] != Convert.ToByte(data[i, j]))
                                pixelData[j, i] = Convert.ToByte(data[i, j]);
                            //System.Diagnostics.Debug.WriteLine(i.ToString() + "-" + j.ToString());
                        }
                    }
                }

                pixelblock.set_PixelData(0, pixelData);

                //Define the location that the upper left corner of the pixel block is to write.
                IPnt upperLeft = new PntClass();
                upperLeft.SetCoords(0, 0);

                //Write the pixel block.
                IRasterEdit rasterEdit = (IRasterEdit)raster;
                rasterEdit.Write(upperLeft, (IPixelBlock)pixelblock);

                //Release rasterEdit explicitly.
                System.Runtime.InteropServices.Marshal.ReleaseComObject(rasterEdit);

                return rasterDataset;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public static IRasterWorkspace2 OpenRasterWorkspace(string pathName)
        {
            try
            {
                IWorkspaceFactory workspaceFactory = new RasterWorkspaceFactoryClass();
                return workspaceFactory.OpenFromFile(pathName, 0) as IRasterWorkspace2;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public static bool UniqueValueRenderer(IColorRamp colorRamp, IRasterLayer rasterLayer, string renderfiled = "Value")
        {
            try
            {
                IRasterUniqueValueRenderer uniqueValueRenderer = new RasterUniqueValueRendererClass();
                IRasterRenderer pRasterRenderer = uniqueValueRenderer as IRasterRenderer;
                pRasterRenderer.Raster = rasterLayer.Raster;
                pRasterRenderer.Update();
                IUniqueValues uniqueValues = new UniqueValuesClass();
                IRasterCalcUniqueValues calcUniqueValues = new RasterCalcUniqueValuesClass();
                calcUniqueValues.AddFromRaster(rasterLayer.Raster, 0, uniqueValues);//iBand=0
                IRasterRendererUniqueValues renderUniqueValues = uniqueValueRenderer as IRasterRendererUniqueValues;
                renderUniqueValues.UniqueValues = uniqueValues;
                uniqueValueRenderer.Field = renderfiled;
                colorRamp.Size = uniqueValues.Count;

                uniqueValueRenderer.HeadingCount = 1;
                uniqueValueRenderer.set_Heading(0, "All Data Value");
                uniqueValueRenderer.set_ClassCount(0, uniqueValues.Count);
                bool pOk;
                colorRamp.CreateRamp(out pOk);
                IRasterRendererColorRamp pRasterRendererColorRamp = uniqueValueRenderer as IRasterRendererColorRamp;
                pRasterRendererColorRamp.ColorRamp = colorRamp;
                for (int i = 0; i < uniqueValues.Count; i++)
                {
                    uniqueValueRenderer.AddValue(0, i, uniqueValues.get_UniqueValue(i));
                    uniqueValueRenderer.set_Label(0, i, uniqueValues.get_UniqueValue(i).ToString());
                    IFillSymbol fs = new SimpleFillSymbol();
                    //fs.Color = colorRamp.get_Color(i);
                    IColor color = new RgbColorClass();
                    if ((Convert.ToByte(uniqueValues.get_UniqueValue(i)) == 255) || (Convert.ToByte(uniqueValues.get_UniqueValue(i)) == 0))
                    {
                        color.NullColor = true;
                        color.Transparency = 0;
                        fs.Color = color;
                    }
                    else
                    {
                        IRgbColor rgbColor = color as IRgbColor;
                        rgbColor.Red = 0;
                        rgbColor.Green = 255;
                        rgbColor.Blue = 0;
                        fs.Color = rgbColor as IColor;
                    }
                    uniqueValueRenderer.set_Symbol(0, i, fs as ISymbol);
                }
                pRasterRenderer.Update();
                rasterLayer.Renderer = pRasterRenderer;
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
