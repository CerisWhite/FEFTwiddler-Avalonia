using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using FEFTwiddler.Enums;

namespace FEFTwiddler.GUI.Convoy
{
    public partial class ConvoyMain : UserControl
    {
        private Model.IChapterSave? _chapterSave;

        public ConvoyMain()
        {
            InitializeComponent();
        }

        public void LoadChapterSave(Model.IChapterSave chapterSave)
        {
            _chapterSave = chapterSave;

            var allItems = Data.Database.Items.GetAll()
                .Where(x => x.Type != ItemType.Unknown || x.ItemID == Item.None)
                .OrderBy(x => x.DisplayName)
                .ToList();

            cmbItem.ItemsSource = allItems;
            cmbItem.SelectedItem = allItems.FirstOrDefault(x => x.ItemID == Item.None);

            cmbItem.SelectionChanged += OnItemChanged;
            btnAdd.IsEnabled = false;

            FillAllPages();
            UpdateConvoyCount();
        }

        private void FillAllPages()
        {
            flwSword.Children.Clear(); flwLance.Children.Clear(); flwAxe.Children.Clear();
            flwShuriken.Children.Clear(); flwBow.Children.Clear(); flwTome.Children.Clear();
            flwStaff.Children.Clear(); flwStone.Children.Clear(); flwConsumable.Children.Clear();

            foreach (var item in _chapterSave!.ConvoyRegion.Convoy.OrderBy(x => x.ItemID))
            {
                try
                {
                    var itemData = Data.Database.Items.GetByID(item.ItemID);
                    var panel = MakePanel(item);
                    GetStack(itemData.Type)?.Children.Add(panel);
                }
                catch { /* skip unrecognized item IDs */ }
            }
        }

        private ConvoyItemPanel MakePanel(Model.ConvoyItem item)
        {
            var panel = new ConvoyItemPanel();
            panel.LoadItem(_chapterSave!, item);
            panel.OnCombineItems = (src, dest) => CombineItems(src, dest);
            panel.OnCountUpdated = UpdateConvoyCount;
            return panel;
        }

        private void OnItemChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (cmbItem.SelectedItem is not Data.Item itemData) return;

            try { picIcon.Source = new Bitmap(AssetLoader.Open(new Uri(itemData.GetIconPath()))); }
            catch { picIcon.Source = null; }

            if (itemData.ItemID == Item.None) { btnAdd.IsEnabled = false; return; }

            EnforceConvoyLimits();

            if (itemData.Type.HasForges())
            {
                numCharges.IsEnabled = true; numCharges.Minimum = Model.Item.MinForges; numCharges.Maximum = Model.Item.MaxForges; numCharges.Value = Model.Item.MinForges;
            }
            else if (itemData.Type.HasCharges())
            {
                numCharges.IsEnabled = true; numCharges.Minimum = Model.Item.MinUses; numCharges.Maximum = itemData.MaximumUses; numCharges.Value = itemData.MaximumUses;
            }
            else
            {
                numCharges.IsEnabled = false; numCharges.Minimum = 0; numCharges.Maximum = 0; numCharges.Value = 0;
            }
        }

        private void BtnAdd_Click(object? sender, RoutedEventArgs e)
        {
            if (cmbItem.SelectedItem is not Data.Item itemData || itemData.ItemID == Item.None) return;

            var charges = (byte)(numCharges.Value ?? 0);
            var existing = _chapterSave!.ConvoyRegion.Convoy
                .FirstOrDefault(x => x.ItemID == itemData.ItemID && x.Uses == charges && !x.IsNamed);

            if (existing == null)
            {
                existing = Model.ConvoyItem.Create();
                existing.ItemID = itemData.ItemID;
                _chapterSave.ConvoyRegion.Convoy.Add(existing);
            }

            if (itemData.Type.HasForges()) existing.Uses = (byte)Math.Min(charges, Model.Item.MaxForges);
            else if (itemData.Type.HasCharges()) existing.Uses = (byte)Math.Min(charges, itemData.MaximumUses);

            existing.Quantity = (byte)Math.Min(existing.Quantity + (numQuantity.Value ?? 1), Model.Item.MaxQuantity);

            RefillPage(itemData.Type);
            UpdateConvoyCount();
        }

        private void RefillPage(ItemType type)
        {
            var stack = GetStack(type);
            if (stack == null) return;
            stack.Children.Clear();
            foreach (var item in _chapterSave!.ConvoyRegion.Convoy
                .Where(x => Data.Database.Items.GetByID(x.ItemID).Type == type)
                .OrderBy(x => x.ItemID))
            {
                stack.Children.Add(MakePanel(item));
            }
        }

        private void EnforceConvoyLimits()
        {
            btnAdd.IsEnabled = _chapterSave!.ConvoyRegion.Convoy.Count < Model.ChapterSaveRegions.ConvoyRegion.MaxConvoyCount;
        }

        public void UpdateConvoyCount()
        {
            lblConvoySize.Text = $"{_chapterSave!.ConvoyRegion.Convoy.Count}/{Model.ChapterSaveRegions.ConvoyRegion.MaxConvoyCount}";
        }

        public void CombineItems(Model.ConvoyItem src, Model.ConvoyItem dest)
        {
            dest.Quantity = (byte)Math.Min(dest.Quantity + src.Quantity, Model.Item.MaxQuantity);
            _chapterSave!.ConvoyRegion.Convoy.Remove(src);
            Utils.WeaponNameUtil.RemoveWeaponNameIfUnused(_chapterSave, src.WeaponNameID);

            var itemType = Data.Database.Items.GetByID(dest.ItemID).Type;
            RefillPage(itemType);
            UpdateConvoyCount();
        }

        private async void BtnEmptyConvoy_Click(object? sender, RoutedEventArgs e)
        {
            var win = TopLevel.GetTopLevel(this) as Window;
            if (win == null || _chapterSave == null) return;
            var result = await MsgBox.ShowYesNo(win,
                "This will remove ALL items from your convoy, regardless of their importance to the plot or sentimental value. Proceed?",
                "Empty convoy?");
            if (!result) return;
            _chapterSave.ConvoyRegion.Convoy.Clear();
            Utils.WeaponNameUtil.RemoveAllUnusedWeaponNames(_chapterSave);
            FillAllPages();
            UpdateConvoyCount();
        }

        private StackPanel? GetStack(ItemType type) => type switch
        {
            ItemType.Sword => flwSword,
            ItemType.Lance => flwLance,
            ItemType.Axe => flwAxe,
            ItemType.Shuriken => flwShuriken,
            ItemType.Bow => flwBow,
            ItemType.Tome => flwTome,
            ItemType.Staff => flwStaff,
            ItemType.Stone or ItemType.NPC => flwStone,
            ItemType.Consumable or ItemType.Held => flwConsumable,
            _ => flwConsumable
        };
    }
}
