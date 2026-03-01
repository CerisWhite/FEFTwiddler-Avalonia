using System;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using FEFTwiddler.Enums;

namespace FEFTwiddler.GUI.Convoy
{
    public partial class ConvoyItemPanel : UserControl
    {
        private Model.IChapterSave? _chapterSave;
        private Model.ConvoyItem? _item;
        public Model.ConvoyItem? Item => _item;

        // Parent convoy reference for combining items and count updates
        public Action<Model.ConvoyItem, Model.ConvoyItem>? OnCombineItems;
        public Action? OnCountUpdated;
        public Func<Model.ConvoyItem, bool>? OnRemoveItem;

        public ConvoyItemPanel()
        {
            InitializeComponent();
        }

        public void LoadItem(Model.IChapterSave chapterSave, Model.ConvoyItem item)
        {
            _chapterSave = chapterSave;
            _item = item;
            Repopulate();
        }

        public void Repopulate()
        {
            if (_item == null) return;
            var loading = true;
            numCharges.ValueChanged -= OnChargesChanged;
            numQuantity.ValueChanged -= OnQuantityChanged;
            numCharges.LostFocus -= OnChargesLostFocus;

            var itemData = Data.Database.Items.GetByID(_item.ItemID);

            if (_item.IsNamed)
            {
                lblName.Foreground = Brushes.Blue;
                var wn = _chapterSave!.WeaponNameRegion.WeaponNames.FirstOrDefault(x => x.ID == _item.WeaponNameID);
                lblName.Text = wn?.Name ?? itemData.DisplayName;
            }
            else
            {
                lblName.Foreground = Brushes.Black;
                lblName.Text = itemData.DisplayName;
            }

            try { picIcon.Source = new Bitmap(AssetLoader.Open(new Uri(itemData.GetIconPath()))); }
            catch { picIcon.Source = null; }

            if (itemData.Type.HasCharges())
            {
                numCharges.IsEnabled = true;
                numCharges.Minimum = Model.Item.MinUses; numCharges.Maximum = itemData.MaximumUses;
                numCharges.Value = _item.Uses;
                lblPlus.Text = ""; lblMaxCharges.Text = "/ " + itemData.MaximumUses;
            }
            else if (itemData.Type.HasForges())
            {
                numCharges.IsEnabled = true;
                numCharges.Minimum = Model.Item.MinForges; numCharges.Maximum = Model.Item.MaxForges;
                numCharges.Value = _item.Uses;
                lblPlus.Text = "+"; lblMaxCharges.Text = "";
            }
            else
            {
                numCharges.IsEnabled = false;
                numCharges.Minimum = 0; numCharges.Maximum = 0; numCharges.Value = _item.Uses;
                lblPlus.Text = ""; lblMaxCharges.Text = "";
            }

            numQuantity.Value = _item.Quantity;

            numCharges.ValueChanged += OnChargesChanged;
            numQuantity.ValueChanged += OnQuantityChanged;
            numCharges.LostFocus += OnChargesLostFocus;
        }

        private void OnChargesChanged(object? sender, NumericUpDownValueChangedEventArgs e)
        {
            if (_item != null) _item.Uses = (byte)(numCharges.Value ?? 0);
        }

        private void OnChargesLostFocus(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (_item == null || _chapterSave == null) return;
            var same = _chapterSave.ConvoyRegion.Convoy
                .FirstOrDefault(x => x != _item && x.ItemID == _item.ItemID && x.Uses == _item.Uses && x.WeaponNameID == _item.WeaponNameID && x.IsNamed == _item.IsNamed);
            if (same != null) OnCombineItems?.Invoke(_item, same);
        }

        private void OnQuantityChanged(object? sender, NumericUpDownValueChangedEventArgs e)
        {
            if (_item == null || _chapterSave == null) return;
            _item.Quantity = (byte)(numQuantity.Value ?? 0);
            if (_item.Quantity == 0)
            {
                _chapterSave.ConvoyRegion.Convoy.Remove(_item);
                Utils.WeaponNameUtil.RemoveWeaponNameIfUnused(_chapterSave, _item.WeaponNameID);
                OnCountUpdated?.Invoke();
                (Parent as Avalonia.Controls.Panel)?.Children.Remove(this);
            }
        }
    }
}
