using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using DevExpress.DashboardCommon;
using DevExpress.XtraEditors;

namespace Dashboard_CreateBubbleMap {
    public partial class Form1 : XtraForm {
        public Form1() {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e) {
            // Creates a new dashboard and a data source for this dashboard.
            Dashboard dashboard = new Dashboard();
            dashboard.AddDataSource("Data Source 1", GetData());

            // Creates a Bubble Map dashboard item and specifies its data source.
            BubbleMapDashboardItem bubbleMap = new BubbleMapDashboardItem();
            bubbleMap.DataSource = dashboard.DataSources[0];

            // Loads the map of the world.
            bubbleMap.Area = ShapefileArea.WorldCountries;

            // Provides cities' coordinates.
            bubbleMap.Latitude = new Dimension("glat");
            bubbleMap.Longitude = new Dimension("glon");

            // Specifies measures used to evaluate bubble weights and colors.
            bubbleMap.Weight = new Measure("mag");
            bubbleMap.Color = new Measure("yr", SummaryType.Min); 
            bubbleMap.Color.NumericFormat.FormatType = DataItemNumericFormatType.General;

            // Provides additional values displayed within bubble tooltips.
            bubbleMap.TooltipDimensions.Add(new Dimension("glat"));
            bubbleMap.TooltipDimensions.Add(new Dimension("glon"));

            // Specifies a custom scale and shows map legends.
            CustomizeScale(bubbleMap);
            ShowLegends(bubbleMap);

            // Adds the Bubble Map dashboard item to the dashboard and opens this
            // dashboard in the Dashboard Viewer.
            dashboard.Items.Add(bubbleMap);
            dashboardViewer1.Dashboard = dashboard;
        }

        private void CustomizeScale(BubbleMapDashboardItem map) {
            CustomScale customScale = new CustomScale();
            List<double> rangeStops = new List<double>();

            // Specifies that the absolute scale is used to define a set of range stops.
            customScale.IsPercent = false;

            // Specifies custom range stops.   
            rangeStops.Add(1960);
            rangeStops.Add(1970);
            rangeStops.Add(1980);
            rangeStops.Add(1990);            

            // Adds custom range stops to a custom scale.  
            customScale.RangeStops.AddRange(rangeStops);

            map.ColorScale = customScale;
        }

        private void ShowLegends(BubbleMapDashboardItem map) {
            // Enables map legends and specifies its position and orientation.
            map.Legend.Visible = true;
            map.Legend.Orientation = MapLegendOrientation.Horizontal;
            map.WeightedLegend.Visible = true;
            map.WeightedLegend.Position = MapLegendPosition.BottomLeft;
        }

        public DataTable GetData() {
            DataSet xmlDataSet = new DataSet();
            xmlDataSet.ReadXml(@"..\..\Data\Earthquakes.xml");
            return xmlDataSet.Tables[0];
        }

    }
}
