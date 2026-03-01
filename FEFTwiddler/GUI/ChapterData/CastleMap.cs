using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace FEFTwiddler.GUI.ChapterData
{
    public class CastleMap : Control
    {
        private Model.ChapterSaveRegions.MyCastleRegion? _castleRegion;
        private Dictionary<Model.Building, Rect> _virtualMap = new();
        private Model.Building? _selectedBuilding;
        private Data.Building? _selectedBuildingData;

        private const float Scale = 3.0f;
        private const int VirtualCellWidth = 6;
        private const int VirtualCellHeight = 6;

        private Point _mousePosition = new Point(-1, -1);

        public CastleMap()
        {
            PointerMoved += OnPointerMoved;
            PointerExited += OnPointerExited;
            PointerPressed += OnPointerPressed;
            Width = Scale * VirtualCellWidth * 20;
            Height = Scale * VirtualCellHeight * 20;
        }

        public void LoadCastleRegion(Model.ChapterSaveRegions.MyCastleRegion castleRegion)
        {
            _castleRegion = castleRegion;
            _virtualMap = new Dictionary<Model.Building, Rect>();
            foreach (var building in _castleRegion.Buildings)
            {
                var data = Data.Database.Buildings.GetByID(building.BuildingID);
                _virtualMap.Add(building, new Rect(
                    VirtualCellWidth * Shift(building.LeftPosition),
                    VirtualCellHeight * Shift(building.TopPosition),
                    VirtualCellWidth * data.Size,
                    VirtualCellHeight * data.Size));
            }
            InvalidateVisual();
        }

        public override void Render(DrawingContext ctx)
        {
            base.Render(ctx);
            if (_castleRegion == null) return;

            DrawMapBackground(ctx);
            DrawBuildings(ctx);
            DrawSelectionOutline(ctx);
            DrawHoverHighlight(ctx);
        }

        private void DrawMapBackground(DrawingContext ctx)
        {
            var mapName = _castleRegion!.CastleMap switch
            {
                Enums.CastleMap.Hoshidan => "Map_HoshidanStyle",
                Enums.CastleMap.WindTribe => "Map_WindTribeStyle",
                Enums.CastleMap.Izumite => "Map_IzumiteStyle",
                Enums.CastleMap.Nohrian => "Map_NohrianStyle",
                Enums.CastleMap.Chevois => "Map_ChevoisStyle",
                Enums.CastleMap.Nestrian => "Map_NestrianStyle",
                _ => "Map_HoshidanStyle"
            };

            try
            {
                var uri = new Uri($"avares://FEFTwiddler/Resources/Images/{mapName}.png");
                var bitmap = new Bitmap(AssetLoader.Open(uri));
                ctx.DrawImage(bitmap, new Rect(0, 0, Bounds.Width, Bounds.Height));
            }
            catch
            {
                ctx.FillRectangle(Brushes.DarkGreen, new Rect(0, 0, Bounds.Width, Bounds.Height));
            }
        }

        private void DrawBuildings(DrawingContext ctx)
        {
            if (_castleRegion == null) return;

            var buildingBrush = new SolidColorBrush(Color.FromArgb(64, 0, 0, 0));
            var triangleBrush = new SolidColorBrush(Color.FromArgb(128, 0, 0, 0));
            var outlinePen = new Pen(Brushes.Black, 1);

            foreach (var building in _castleRegion.Buildings)
            {
                var data = Data.Database.Buildings.GetByID(building.BuildingID);

                float x = Scale * Shift(building.LeftPosition) * VirtualCellWidth;
                float y = Scale * Shift(building.TopPosition) * VirtualCellHeight;
                float w = Scale * data.Size * VirtualCellWidth;
                float h = Scale * data.Size * VirtualCellHeight;

                var rect = new Rect(x, y, w, h);
                ctx.FillRectangle(buildingBrush, rect);
                ctx.DrawRectangle(outlinePen, rect);

                // Direction arrow
                var pts = GetArrowPoints(building, data);
                if (pts != null)
                {
                    var geo = new StreamGeometry();
                    using (var sgc = geo.Open())
                    {
                        sgc.BeginFigure(pts[0], true);
                        sgc.LineTo(pts[1]);
                        sgc.LineTo(pts[2]);
                        sgc.EndFigure(true);
                    }
                    ctx.DrawGeometry(triangleBrush, null, geo);
                }
            }
        }

        private Point[]? GetArrowPoints(Model.Building building, Data.Building data)
        {
            float lx = Scale * Shift(building.LeftPosition) * VirtualCellWidth;
            float ty = Scale * Shift(building.TopPosition) * VirtualCellHeight;
            float cw = Scale * VirtualCellWidth;
            float ch = Scale * VirtualCellHeight;

            return building.DirectionFacing switch
            {
                Enums.BuildingDirection.Down => new[]
                {
                    new Point(lx + data.Size * cw * 0.5f, ty + data.Size * ch),
                    new Point(lx + data.Size * cw * 0.5f - cw * 0.25f, ty + data.Size * ch - ch * 0.5f),
                    new Point(lx + data.Size * cw * 0.5f + cw * 0.25f, ty + data.Size * ch - ch * 0.5f),
                },
                Enums.BuildingDirection.Up => new[]
                {
                    new Point(lx + data.Size * cw * 0.5f, ty),
                    new Point(lx + data.Size * cw * 0.5f - cw * 0.25f, ty + ch * 0.5f),
                    new Point(lx + data.Size * cw * 0.5f + cw * 0.25f, ty + ch * 0.5f),
                },
                Enums.BuildingDirection.Left => new[]
                {
                    new Point(lx, ty + data.Size * ch * 0.5f),
                    new Point(lx + cw * 0.5f, ty + data.Size * ch * 0.5f - ch * 0.25f),
                    new Point(lx + cw * 0.5f, ty + data.Size * ch * 0.5f + ch * 0.25f),
                },
                Enums.BuildingDirection.Right => new[]
                {
                    new Point(lx + data.Size * cw, ty + data.Size * ch * 0.5f),
                    new Point(lx + data.Size * cw - cw * 0.5f, ty + data.Size * ch * 0.5f - ch * 0.25f),
                    new Point(lx + data.Size * cw - cw * 0.5f, ty + data.Size * ch * 0.5f + ch * 0.25f),
                },
                _ => null
            };
        }

        private void DrawHoverHighlight(DrawingContext ctx)
        {
            if (_mousePosition.X < 0) return;

            float physW = Scale * VirtualCellWidth;
            float physH = Scale * VirtualCellHeight;
            float physX = (float)Math.Floor(_mousePosition.X / physW) * physW;
            float physY = (float)Math.Floor(_mousePosition.Y / physH) * physH;

            ctx.FillRectangle(new SolidColorBrush(Color.FromArgb(128, 255, 255, 0)), new Rect(physX, physY, physW, physH));
        }

        private void DrawSelectionOutline(DrawingContext ctx)
        {
            if (_selectedBuilding == null || _selectedBuildingData == null) return;

            float physW = Scale * VirtualCellWidth * _selectedBuildingData.Size;
            float physH = Scale * VirtualCellHeight * _selectedBuildingData.Size;
            float physX = Scale * VirtualCellWidth * Shift(_selectedBuilding.LeftPosition);
            float physY = Scale * VirtualCellHeight * Shift(_selectedBuilding.TopPosition);

            ctx.DrawRectangle(new Pen(Brushes.Red, Scale), new Rect(physX, physY, physW, physH));
        }

        private void OnPointerMoved(object? sender, PointerEventArgs e)
        {
            _mousePosition = e.GetPosition(this);
            InvalidateVisual();
        }

        private void OnPointerExited(object? sender, PointerEventArgs e)
        {
            _mousePosition = new Point(-1, -1);
            InvalidateVisual();
        }

        private void OnPointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (_castleRegion == null) return;

            var pos = e.GetPosition(this);
            int virtX = (int)(pos.X / Scale);
            int virtY = (int)(pos.Y / Scale);

            var hit = _virtualMap.Where(x => x.Value.Contains(new Point(virtX, virtY))).Select(x => (Model.Building?)x.Key).FirstOrDefault();

            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                if (hit == null) DeselectBuilding();
                else SelectBuilding(hit);
            }
            else if (e.GetCurrentPoint(this).Properties.IsRightButtonPressed)
            {
                if (hit != null) RotateBuilding(hit);
                InvalidateVisual();
            }
        }

        private void SelectBuilding(Model.Building building)
        {
            _selectedBuilding = building;
            _selectedBuildingData = Data.Database.Buildings.GetByID(building.BuildingID);
            InvalidateVisual();
            SelectionChanged?.Invoke(this, $"Selected: {building.BuildingID}");
        }

        private void DeselectBuilding()
        {
            _selectedBuilding = null;
            _selectedBuildingData = null;
            InvalidateVisual();
            SelectionChanged?.Invoke(this, "Selected: (none)");
        }

        private void RotateBuilding(Model.Building building)
        {
            building.DirectionFacing = building.DirectionFacing switch
            {
                Enums.BuildingDirection.Down => Enums.BuildingDirection.Left,
                Enums.BuildingDirection.Left => Enums.BuildingDirection.Up,
                Enums.BuildingDirection.Up => Enums.BuildingDirection.Right,
                Enums.BuildingDirection.Right => Enums.BuildingDirection.Down,
                _ => building.DirectionFacing
            };
        }

        private static int Shift(int pos) => pos - 1;

        public event EventHandler<string>? SelectionChanged;
    }
}
