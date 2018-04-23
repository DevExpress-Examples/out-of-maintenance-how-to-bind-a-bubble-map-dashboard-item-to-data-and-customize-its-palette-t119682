Imports System
Imports System.Collections.Generic
Imports System.Data
Imports DevExpress.DashboardCommon
Imports DevExpress.XtraEditors
Imports DevExpress.DataAccess.Sql
Imports DevExpress.DataAccess.ConnectionParameters

Namespace Dashboard_CreateBubbleMap
    Partial Public Class Form1
        Inherits XtraForm

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
            ' Creates a new dashboard and a data source for this dashboard.
            Dim dashboard As New Dashboard()
            Dim dataSource As New DashboardSqlDataSource()
            dataSource.ConnectionParameters = New XmlFileConnectionParameters("..\..\Data\Earthquakes.xml")
            Dim sqlQuery As New TableQuery("Earthquakes")
            sqlQuery.AddTable("Row").SelectColumns("glat", "glon", "mag", "yr")
            dataSource.Queries.Add(sqlQuery)
            dashboard.DataSources.Add(dataSource)

            ' Creates a Bubble Map dashboard item and specifies its data source.
            Dim bubbleMap As New BubbleMapDashboardItem()
            bubbleMap.DataSource = dashboard.DataSources(0)
            bubbleMap.DataMember = "Earthquakes"

            ' Loads the map of the world.
            bubbleMap.Area = ShapefileArea.WorldCountries

            ' Provides cities' coordinates.
            bubbleMap.Latitude = New Dimension("glat")
            bubbleMap.Longitude = New Dimension("glon")

            ' Specifies measures used to evaluate bubble weights and colors.
            bubbleMap.Weight = New Measure("mag")
            bubbleMap.Color = New Measure("yr", SummaryType.Min)
            bubbleMap.Color.NumericFormat.FormatType = DataItemNumericFormatType.General

            ' Provides additional values displayed within bubble tooltips.
            bubbleMap.TooltipDimensions.Add(New Dimension("glat"))
            bubbleMap.TooltipDimensions.Add(New Dimension("glon"))

            ' Specifies a custom scale and shows map legends.
            CustomizeScale(bubbleMap)
            ShowLegends(bubbleMap)

            ' Adds the Bubble Map dashboard item to the dashboard and opens this
            ' dashboard in the Dashboard Viewer.
            dashboard.Items.Add(bubbleMap)
            dashboardViewer1.Dashboard = dashboard
        End Sub

        Private Sub CustomizeScale(ByVal map As BubbleMapDashboardItem)
            Dim customScale As New CustomScale()
            Dim rangeStops As New List(Of Double)()

            ' Specifies that the absolute scale is used to define a set of range stops.
            customScale.IsPercent = False

            ' Specifies custom range stops.   
            rangeStops.Add(1960)
            rangeStops.Add(1970)
            rangeStops.Add(1980)
            rangeStops.Add(1990)

            ' Adds custom range stops to a custom scale.  
            customScale.RangeStops.AddRange(rangeStops)

            map.ColorScale = customScale
        End Sub

        Private Sub ShowLegends(ByVal map As BubbleMapDashboardItem)
            ' Enables map legends and specifies its position and orientation.
            map.Legend.Visible = True
            map.Legend.Orientation = MapLegendOrientation.Horizontal
            map.WeightedLegend.Visible = True
            map.WeightedLegend.Position = MapLegendPosition.BottomLeft
        End Sub
    End Class
End Namespace
