using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using FEFTwiddler.Enums;
using FEFTwiddler.Extensions;
using FEFTwiddler.Model;

namespace FEFTwiddler.GUI.UnitViewer
{
    public partial class Inventory : UserControl
    {
        private Model.Unit? _unit;
        private InventoryItemPanel[] _slots = null!;

        public Inventory()
        {
            InitializeComponent();
        }

        public void LoadUnit(Model.Unit unit)
        {
            _unit = unit;

            if (_slots == null)
            {
                var allItems = Data.Database.Items.GetAll()
                    .Where(x => x.Type != Enums.ItemType.Unknown || x.ItemID == Enums.Item.None)
                    .OrderBy(x => x.DisplayName)
                    .ToList();

                _slots = new[]
                {
                    new InventoryItemPanel(ItemPic_1, ItemNameBox_1, ItemIsEquipped_1, ItemForgesBox_1, ItemQuantBox_1, ItemHexBox_1, allItems),
                    new InventoryItemPanel(ItemPic_2, ItemNameBox_2, ItemIsEquipped_2, ItemForgesBox_2, ItemQuantBox_2, ItemHexBox_2, allItems),
                    new InventoryItemPanel(ItemPic_3, ItemNameBox_3, ItemIsEquipped_3, ItemForgesBox_3, ItemQuantBox_3, ItemHexBox_3, allItems),
                    new InventoryItemPanel(ItemPic_4, ItemNameBox_4, ItemIsEquipped_4, ItemForgesBox_4, ItemQuantBox_4, ItemHexBox_4, allItems),
                    new InventoryItemPanel(ItemPic_5, ItemNameBox_5, ItemIsEquipped_5, ItemForgesBox_5, ItemQuantBox_5, ItemHexBox_5, allItems),
                };
            }

            _slots[0].LoadItem(_unit.Item_1);
            _slots[1].LoadItem(_unit.Item_2);
            _slots[2].LoadItem(_unit.Item_3);
            _slots[3].LoadItem(_unit.Item_4);
            _slots[4].LoadItem(_unit.Item_5);
        }

        private void BtnMaxForges_Click(object? sender, RoutedEventArgs e)
        {
            foreach (var s in _slots) s.SetForges(7);
        }

        private void BtnMaxCharges_Click(object? sender, RoutedEventArgs e)
        {
            foreach (var s in _slots) s.SetCharges(35);
        }
    }

    internal class InventoryItemPanel
    {
        private readonly Avalonia.Controls.Image _pic;
        private readonly ComboBox _name;
        private readonly CheckBox _equipped;
        private readonly NumericUpDown _forges;
        private readonly NumericUpDown _charges;
        private readonly TextBox _raw;
        private readonly List<Data.Item> _allItems;
        private InventoryItem? _item;
        private bool _updating;
        private bool _eventsBound;

        public InventoryItemPanel(Avalonia.Controls.Image pic, ComboBox name, CheckBox equipped,
            NumericUpDown forges, NumericUpDown charges, TextBox raw, List<Data.Item> allItems)
        {
            _pic = pic; _name = name; _equipped = equipped;
            _forges = forges; _charges = charges; _raw = raw;
            _allItems = allItems;

            _name.ItemsSource = allItems;
        }

        public void LoadItem(InventoryItem item)
        {
            _item = item;
            Update();
            if (!_eventsBound) { BindEvents(); _eventsBound = true; }
        }

        private void Update()
        {
            _updating = true;
            try
            {
                var data = Data.Database.Items.GetByID(_item!.ItemID);
                _name.SelectedItem = _allItems.FirstOrDefault(x => x.ItemID == _item.ItemID);
                _equipped.IsChecked = _item.IsEquipped;

                // Load icon
                try
                {
                    _pic.Source = new Bitmap(AssetLoader.Open(new Uri(data.GetIconPath())));
                }
                catch { _pic.Source = null; }

                if (data.Type.HasCharges())
                {
                    _charges.IsEnabled = true;
                    _charges.Minimum = Model.Item.MinUses; _charges.Maximum = data.MaximumUses;
                    _charges.Value = Math.Max(Model.Item.MinUses, Math.Min(_item.Uses, data.MaximumUses));
                    _forges.IsEnabled = false; _forges.Value = 0;
                    _equipped.IsEnabled = false; _equipped.IsChecked = false;
                }
                else if (data.Type.HasForges())
                {
                    _forges.IsEnabled = true;
                    _forges.Minimum = Model.Item.MinForges; _forges.Maximum = Model.Item.MaxForges;
                    _forges.Value = Math.Max(Model.Item.MinForges, Math.Min(_item.Uses, Model.Item.MaxForges));
                    _charges.IsEnabled = false; _charges.Value = 1;
                    _equipped.IsEnabled = true; _equipped.IsChecked = _item.IsEquipped;
                }
                else
                {
                    _forges.IsEnabled = false; _forges.Value = 0;
                    _charges.IsEnabled = false; _charges.Value = 1;
                    _equipped.IsEnabled = false; _equipped.IsChecked = false;
                    _item.IsEquipped = false;
                }

                _raw.Text = _item.Hex();
            }
            finally { _updating = false; }
        }

        private void BindEvents()
        {
            _name.SelectionChanged += (_, _) =>
            {
                if (_updating || _item == null) return;
                if (_name.SelectedItem is Data.Item itm) { _item.ItemID = itm.ItemID; Update(); }
            };
            _equipped.IsCheckedChanged += (_, _) =>
            {
                if (_updating || _item == null) return;
                _item.IsEquipped = _equipped.IsChecked == true; Update();
            };
            _forges.ValueChanged += (_, _) => { if (!_updating && _item != null) { _item.Uses = (byte)(_forges.Value ?? 0); Update(); } };
            _charges.ValueChanged += (_, _) => { if (!_updating && _item != null) { _item.Uses = (byte)(_charges.Value ?? 1); Update(); } };
            _raw.TextChanged += (_, _) =>
            {
                if (_updating || _item == null) return;
                var bytes = new byte[4];
                if (bytes.TryParseHex(_raw.Text ?? "")) { _item.Reparse(bytes); Update(); }
            };
        }

        public void SetForges(int val)
        {
            if (_item == null) return;
            var data = Data.Database.Items.GetByID(_item.ItemID);
            if (data.Type.HasForges()) { _item.Uses = (byte)Math.Min(val, Model.Item.MaxForges); Update(); }
        }

        public void SetCharges(int val)
        {
            if (_item == null) return;
            var data = Data.Database.Items.GetByID(_item.ItemID);
            if (data.Type.HasCharges()) { _item.Uses = (byte)Math.Min(val, data.MaximumUses); Update(); }
        }
    }
}
