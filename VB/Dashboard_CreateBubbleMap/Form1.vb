Imports System
Imports System.Collections.Generic
Imports DevExpress.DashboardCommon
Imports DevExpress.DataAccess.ConnectionParameters
Imports DevExpress.DataAccess.Sql
Imports DevExpress.XtraEditors

Namespace Dashboard_CreateBubbleMap
	Partial Public Class Form1
		Inherits XtraForm

		Public Sub New()
			InitializeComponent()
		End Sub

		Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
			Dim dashboard As New Dashboard()
			Dim dataSource As New DashboardSqlDataSource()
			dataSource.ConnectionParameters = New XmlFileConnectionParameters("..\..\Data\DashboardEnergyStatictics.xml")
			Dim sqlQuery As SelectQuery = SelectQueryFluentBuilder.AddTable("Countries").SelectColumns("Latitude", "Longitude", "Production", "Import", "Country").Build("Countries")

			dataSource.Queries.Add(sqlQuery)
			dashboard.DataSources.Add(dataSource)

			Dim bubbleMap As New BubbleMapDashboardItem()
			bubbleMap.DataSource = dashboard.DataSources(0)
			bubbleMap.DataMember = "Countries"

			bubbleMap.Area = ShapefileArea.Europe

			bubbleMap.Latitude = New Dimension("Latitude")
			bubbleMap.Longitude = New Dimension("Longitude")

			bubbleMap.Weight = New Measure("Production", SummaryType.Sum)
			bubbleMap.Color = New Measure("Import", SummaryType.Sum)
			bubbleMap.Color.NumericFormat.FormatType = DataItemNumericFormatType.General

			bubbleMap.TooltipDimensions.Add(New Dimension("Country"))

			CustomizeScale(bubbleMap)
			ShowLegends(bubbleMap)

			dashboard.Items.Add(bubbleMap)
			dashboardViewer1.Dashboard = dashboard
		End Sub
		Private Sub CustomizeScale(ByVal map As BubbleMapDashboardItem)
			Dim customScale As New CustomScale()
			Dim rangeStops As New List(Of Double)()

			customScale.IsPercent = False

			rangeStops.Add(20)
			rangeStops.Add(200)
			rangeStops.Add(500)
			rangeStops.Add(2000)
			customScale.RangeStops.AddRange(rangeStops)

			map.ColorScale = customScale
		End Sub
		Private Sub ShowLegends(ByVal map As BubbleMapDashboardItem)
			map.Legend.Visible = True
			map.Legend.Orientation = MapLegendOrientation.Horizontal
			map.WeightedLegend.Visible = True
			map.WeightedLegend.Position = MapLegendPosition.BottomLeft
		End Sub
	End Class
End Namespace
