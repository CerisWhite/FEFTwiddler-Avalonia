using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using FEFTwiddler.Extensions;

namespace FEFTwiddler.GUI
{
    public partial class MainWindow : Window
    {
        private Model.SaveFile? _saveFile;
        private Model.IChapterSave? _chapterSave;
        private Model.GlobalSave? _globalSave;
        private Model.Unit? _selectedUnit;

        private readonly ObservableCollection<Model.Unit> _livingUnits = new();
        private readonly ObservableCollection<Model.Unit> _deadUnits = new();

        public MainWindow()
        {
            InitializeComponent();

            lstLiving.ItemsSource = _livingUnits;
            lstDead.ItemsSource = _deadUnits;

            AddHandler(DragDrop.DropEvent, OnDrop);
            AddHandler(DragDrop.DragEnterEvent, OnDragEnter);
            DragDrop.SetAllowDrop(this, true);
        }

        private void OnDragEnter(object? sender, DragEventArgs e)
        {
            if (e.Data.Contains(DataFormats.Files)) e.DragEffects = DragDropEffects.Copy;
        }

        private void OnDrop(object? sender, DragEventArgs e)
        {
            var files = e.Data.GetFiles();
            if (files != null)
            {
                foreach (var file in files)
                {
                    if (file is IStorageFile sf)
                    {
                        _ = LoadFileAsync(sf.Path.LocalPath);
                        break;
                    }
                }
            }
        }

        #region Load

        private async void OpenFile_Click(object? sender, RoutedEventArgs e)
        {
            var options = new FilePickerOpenOptions
            {
                Title = "Open Save File",
                AllowMultiple = false
            };

            var startupPath = Config.StartupPath;
            if (!string.IsNullOrEmpty(startupPath) && Directory.Exists(startupPath))
            {
                options.SuggestedStartLocation = await StorageProvider.TryGetFolderFromPathAsync(startupPath);
            }

            var results = await StorageProvider.OpenFilePickerAsync(options);
            if (results.Count > 0)
            {
                var path = results[0].Path.LocalPath;
                Config.StartupPath = Path.GetDirectoryName(path) ?? "";
                await LoadFileAsync(path);
            }
        }

        private async Task LoadFileAsync(string path)
        {
            var saveFile = Model.SaveFile.FromPath(path);

            if (saveFile.Type != Enums.SaveFileType.Chapter &&
                saveFile.Type != Enums.SaveFileType.Map &&
                saveFile.Type != Enums.SaveFileType.Global)
            {
                await MsgBox.ShowInfo(this, "This type of save is not supported yet. Only 'Chapter', 'Map', and 'Global' saves are supported right now.");
                return;
            }

            _saveFile = saveFile;
            UpdateTitleBar(path);

            switch (_saveFile.Type)
            {
                case Enums.SaveFileType.Chapter:
                case Enums.SaveFileType.Map:
                    if (_saveFile.Type == Enums.SaveFileType.Chapter) _chapterSave = Model.ChapterSave.FromSaveFile(_saveFile);
                    else _chapterSave = Model.MapSave.FromSaveFile(_saveFile);

                    _globalSave = null;

                    tabChapterData.IsEnabled = true;
                    tabUnitViewer.IsEnabled = true;
                    tabMegacheats.IsEnabled = true;
                    tabConvoy.IsEnabled = true;
                    tabGlobalData.IsEnabled = false;
                    tabControl1.IsEnabled = true;

                    LoadChapterData();
                    LoadUnitViewer();
                    break;

                case Enums.SaveFileType.Global:
                    _globalSave = Model.GlobalSave.FromSaveFile(_saveFile);
                    _chapterSave = null;

                    tabChapterData.IsEnabled = false;
                    tabUnitViewer.IsEnabled = false;
                    tabMegacheats.IsEnabled = false;
                    tabConvoy.IsEnabled = false;
                    tabGlobalData.IsEnabled = true;
                    tabControl1.IsEnabled = true;

                    LoadGlobalData();
                    break;
            }
        }

        private void UpdateTitleBar(string path)
        {
            var directory = Path.GetDirectoryName(path) ?? "";
            if (directory.Length > 80) directory = "..." + directory.Right(80);
            Title = "FEFTwiddler - " + directory + Path.DirectorySeparatorChar + Path.GetFileName(path);
        }

        #endregion

        #region Save

        private async void SaveFile_Click(object? sender, RoutedEventArgs e)
        {
            if (_saveFile == null) { await MsgBox.ShowInfo(this, "No file is loaded"); return; }

            switch (_saveFile.Type)
            {
                case Enums.SaveFileType.Chapter:
                case Enums.SaveFileType.Map:
                    if (_chapterSave == null) { await MsgBox.ShowInfo(this, "No file is loaded"); return; }
                    _chapterSave.Write();
                    break;
                case Enums.SaveFileType.Global:
                    if (_globalSave == null) { await MsgBox.ShowInfo(this, "No file is loaded"); return; }
                    _globalSave.Write();
                    break;
                default:
                    await MsgBox.ShowInfo(this, "No file is loaded"); return;
            }

            await MsgBox.ShowInfo(this, "File saved. A backup was made in the source directory as well.");
        }

        #endregion

        #region Chapter Data

        private void LoadChapterData()
        {
            lblAvatarName.Text = _chapterSave!.Header.AvatarName;

            goldAndPoints1.LoadChapterSave(_chapterSave);
            materials1.LoadChapterSave(_chapterSave);
            megacheatsMain1.LoadChapterSave(_chapterSave);
            difficulty1.LoadChapterSave(_chapterSave);
            convoyMain1.LoadChapterSave(_chapterSave);
            gameProgressMain1.LoadChapterSave(_chapterSave);
        }

        private async void BtnCastleMap_Click(object? sender, RoutedEventArgs e)
        {
            var popup = new ChapterData.CastleViewer(_chapterSave!.MyCastleRegion);
            await popup.ShowDialog(this);
        }

        #endregion

        #region Unit Viewer

        public void LoadUnitViewer(Model.Unit unit)
        {
            LoadUnitViewer();
            SelectUnit(unit);
        }

        public void LoadUnitViewer()
        {
            _livingUnits.Clear();
            _deadUnits.Clear();

            foreach (var unit in _chapterSave!.UnitRegion.Units)
            {
                if (IsDead(unit)) _deadUnits.Add(unit);
                else _livingUnits.Add(unit);
            }

            // Defer pre-selection so the Unit Viewer tab has time to render its controls
            // before LoadUnit is called. In Avalonia, TabControl lazily renders non-active tabs,
            // so NumericUpDown templates won't be applied until the tab is actually shown.
            lstLiving.SelectedItem = null;
            lstDead.SelectedItem = null;
            if (_livingUnits.Count > 0)
                Avalonia.Threading.Dispatcher.UIThread.Post(() => lstLiving.SelectedIndex = 0);

            UpdateUnitCount();
        }

        private void UpdateUnitCount()
        {
            lblUnitCount.Text = $"Units: {_chapterSave!.UnitRegion.Units.Count}/{Model.ChapterSaveRegions.UnitRegion.HardMaxUnits}";
        }

        private bool IsDead(Model.Unit unit)
        {
            return unit.UnitBlock == Enums.UnitBlock.DeadByGameplay || unit.UnitBlock == Enums.UnitBlock.DeadByPlot;
        }

        public void SelectUnit(Model.Unit unit)
        {
            if (_livingUnits.Contains(unit)) lstLiving.SelectedItem = unit;
            else if (_deadUnits.Contains(unit)) lstDead.SelectedItem = unit;
        }

        private void LstLiving_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            var unit = lstLiving.SelectedItem as Model.Unit;
            if (unit == null) return;
            _selectedUnit = unit;
            lstDead.SelectedItem = null;
            LoadUnit(unit);
        }

        private void LstDead_SelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            var unit = lstDead.SelectedItem as Model.Unit;
            if (unit == null) return;
            _selectedUnit = unit;
            lstLiving.SelectedItem = null;
            LoadUnit(unit);
        }

        public void DeselectUnit()
        {
            lstLiving.SelectedItem = null;
            lstDead.SelectedItem = null;
            _selectedUnit = null;

            lblBlanketMessage.Text = "No unit is selected.";
            unitViewerBlanket.IsVisible = true;
        }

        private void LoadUnit(Model.Unit? unit)
        {
            if (unit == null) return;

            var message = "";

            lblName.Text = unit.GetDisplayName();

            if (!Enum.IsDefined(typeof(Enums.Character), unit.CharacterID) ||
                !Enum.IsDefined(typeof(Enums.Class), unit.ClassID))
            {
                lblUsesCustomData.IsVisible = true;
            }
            else
            {
                lblUsesCustomData.IsVisible = false;
            }

            try { classAndLevel1.LoadUnit(_selectedUnit!); }
            catch { message += Environment.NewLine + "Error loading Class and Level data"; }

            try { stats1.LoadUnit(_selectedUnit!); }
            catch { message += Environment.NewLine + "Error loading Stats data"; }

            try { unitBlockInfo1.LoadUnit(_chapterSave!, _selectedUnit!); }
            catch { message += Environment.NewLine + "Error loading Unit Block Info data"; }

            try { flags1.LoadUnit(_selectedUnit!); }
            catch { message += Environment.NewLine + "Error loading Flags data"; }

            try { battleData1.LoadUnit(_selectedUnit!); }
            catch { message += Environment.NewLine + "Error loading Battle data"; }

            try { skills1.LoadUnit(_selectedUnit!); }
            catch { message += Environment.NewLine + "Error loading Skills data"; }

            try { inventory1.LoadUnit(_selectedUnit!); }
            catch { message += Environment.NewLine + "Error loading Inventory data"; }

            try { accessories1.LoadUnit(_selectedUnit!); }
            catch { message += Environment.NewLine + "Error loading Accessories data"; }

            try { hairColor1.LoadUnit(_selectedUnit!); }
            catch { message += Environment.NewLine + "Error loading Hair Color data"; }

            try { weaponExperience1.LoadUnit(_selectedUnit!); }
            catch { message += Environment.NewLine + "Error loading Weapon Experience data"; }

            try { dragonVein1.LoadUnit(_selectedUnit!); }
            catch { message += Environment.NewLine + "Error loading Dragon Vein data"; }

            if (message.Length > 0)
            {
                message = "One or more values is invalid for this unit. You can still use the hex editor, though." + Environment.NewLine + Environment.NewLine + message;
                lblBlanketMessage.Text = message;
                unitViewerBlanket.IsVisible = true;
            }
            else
            {
                unitViewerBlanket.IsVisible = false;
            }

            // Support button
            var supportData = Data.Database.Characters.GetByID(_selectedUnit!.CharacterID)?.SupportPool;
            btnSupport.IsEnabled = supportData != null &&
                                   _selectedUnit.RawNumberOfSupports == supportData.Length &&
                                   supportData.Length > 0;
        }

        private async void BtnImportUnit_Click(object? sender, RoutedEventArgs e)
        {
            if (_chapterSave!.UnitRegion.Units.Count >= Model.ChapterSaveRegions.UnitRegion.HardMaxUnits)
            {
                await MsgBox.ShowInfo(this, "You already have the maximum of " + Model.ChapterSaveRegions.UnitRegion.HardMaxUnits + " units. Please remove one before adding another.");
                return;
            }

            if (_chapterSave.UnitRegion.Units.Count == Model.ChapterSaveRegions.UnitRegion.SoftMaxUnits)
            {
                await MsgBox.ShowInfo(this, "The game normally does not allow you to recruit more than " + Model.ChapterSaveRegions.UnitRegion.SoftMaxUnits + " units. It's possible to add more, but do so at your own risk.");
            }

            var options = new FilePickerOpenOptions
            {
                Title = "Import Unit",
                AllowMultiple = false,
                FileTypeFilter = new[] { new FilePickerFileType("FE14 Unit") { Patterns = new[] { "*.fe14unit" } } }
            };

            var unitPath = Config.UnitPath;
            if (!string.IsNullOrEmpty(unitPath) && Directory.Exists(unitPath))
                options.SuggestedStartLocation = await StorageProvider.TryGetFolderFromPathAsync(unitPath);

            var results = await StorageProvider.OpenFilePickerAsync(options);
            if (results.Count > 0)
            {
                var path = results[0].Path.LocalPath;
                Config.UnitPath = Path.GetDirectoryName(path) ?? "";

                var unit = Model.Unit.FromPath(path);
                unit.UnitBlock = Enums.UnitBlock.Living;
                Utils.UnitUtil.RemoveNamesFromHeldWeaponsWithInvalidNames(_chapterSave, unit);
                Utils.UnitUtil.FixBlock(unit);
                Utils.UnitUtil.Undeploy(unit);
                _chapterSave.UnitRegion.Units.Add(unit);

                LoadUnitViewer();
                SelectUnit(unit);
            }
        }

        private async void BtnExportUnit_Click(object? sender, RoutedEventArgs e)
        {
            var options = new FilePickerSaveOptions
            {
                Title = "Export Unit",
                DefaultExtension = "fe14unit",
                FileTypeChoices = new[] { new FilePickerFileType("FE14 Unit") { Patterns = new[] { "*.fe14unit" } } }
            };

            var unitPath = Config.UnitPath;
            if (!string.IsNullOrEmpty(unitPath) && Directory.Exists(unitPath))
                options.SuggestedStartLocation = await StorageProvider.TryGetFolderFromPathAsync(unitPath);

            var result = await StorageProvider.SaveFilePickerAsync(options);
            if (result != null)
            {
                Config.UnitPath = Path.GetDirectoryName(result.Path.LocalPath) ?? "";
                _selectedUnit!.ToPath(result.Path.LocalPath);
            }
        }

        private async void BtnDeleteUnit_Click(object? sender, RoutedEventArgs e)
        {
            bool confirm = await MsgBox.ShowYesNo(this, "Are you sure you want to remove this unit?", "Remove unit?");
            if (!confirm) return;

            if (_livingUnits.Count == 1 && _livingUnits.Contains(_selectedUnit!))
            {
                bool confirm2 = await MsgBox.ShowYesNo(this, "This is your last living unit. No good can possibly come from removing this unit. Proceed anyway?", "Remove your last living unit?");
                if (!confirm2) return;
            }
            else if (Enum.IsDefined(typeof(Enums.Character), _selectedUnit!.CharacterID) &&
                     Data.Database.Characters.GetByID(_selectedUnit.CharacterID).IsCorrin &&
                     !_selectedUnit.IsEinherjar)
            {
                bool confirm2 = await MsgBox.ShowYesNo(this, "You are about to remove a non-Einherjar Corrin. There's no telling what effect this may have on your game. Proceed?", "Remove Corrin?");
                if (!confirm2) return;
            }

            _chapterSave!.UnitRegion.Units.Remove(_selectedUnit!);
            _livingUnits.Remove(_selectedUnit!);
            _deadUnits.Remove(_selectedUnit!);
            DeselectUnit();
            UpdateUnitCount();
        }

        private async void BtnOpenHexEditor_Click(object? sender, RoutedEventArgs e)
        {
            var hex = new UnitViewer.HexEditor(_selectedUnit!);
            await hex.ShowDialog(this);
            LoadUnit(_selectedUnit);
        }

        private async void BtnSupport_Click(object? sender, RoutedEventArgs e)
        {
            var supportEditor = new UnitViewer.Supports(_chapterSave!, _selectedUnit!);
            await supportEditor.ShowDialog(this);
            LoadUnit(_selectedUnit);
        }

        private async void BtnTraits_Click(object? sender, RoutedEventArgs e)
        {
            var traits = new UnitViewer.Traits(_selectedUnit!);
            await traits.ShowDialog(this);
            LoadUnit(_selectedUnit);
        }

        #endregion

        #region Global Data

        private void LoadGlobalData()
        {
            globalDataMain1.LoadGlobalSave(_globalSave!);
        }

        #endregion

        #region Compression

        private async void DecompressFile_Click(object? sender, RoutedEventArgs e)
        {
            var options = new FilePickerOpenOptions { Title = "Decompress Save", AllowMultiple = false };

            var startupPath = Config.StartupPath;
            if (!string.IsNullOrEmpty(startupPath) && Directory.Exists(startupPath))
                options.SuggestedStartLocation = await StorageProvider.TryGetFolderFromPathAsync(startupPath);

            var results = await StorageProvider.OpenFilePickerAsync(options);
            if (results.Count == 0) return;

            var path = results[0].Path.LocalPath;
            Config.StartupPath = Path.GetDirectoryName(path) ?? "";

            var saveFile = Model.SaveFile.FromPath(path);
            if (!(saveFile.Type == Enums.SaveFileType.Chapter || saveFile.Type == Enums.SaveFileType.Map ||
                  saveFile.Type == Enums.SaveFileType.Global || saveFile.Type == Enums.SaveFileType.Exchange ||
                  saveFile.Type == Enums.SaveFileType.Versus))
            {
                await MsgBox.ShowInfo(this, "This type of save is not supported yet.");
                return;
            }

            saveFile.Decompress();
            await MsgBox.ShowInfo(this, "Done! Decompressed save written to the original filename but with _dec on the end.");
        }

        private async void CompressFile_Click(object? sender, RoutedEventArgs e)
        {
            var options = new FilePickerOpenOptions { Title = "Compress Save", AllowMultiple = false };

            var startupPath = Config.StartupPath;
            if (!string.IsNullOrEmpty(startupPath) && Directory.Exists(startupPath))
                options.SuggestedStartLocation = await StorageProvider.TryGetFolderFromPathAsync(startupPath);

            var results = await StorageProvider.OpenFilePickerAsync(options);
            if (results.Count == 0) return;

            var path = results[0].Path.LocalPath;
            Config.StartupPath = Path.GetDirectoryName(path) ?? "";

            var saveFile = Model.SaveFile.FromPath(path);
            if (!(saveFile.Type == Enums.SaveFileType.Chapter || saveFile.Type == Enums.SaveFileType.Map ||
                  saveFile.Type == Enums.SaveFileType.Global || saveFile.Type == Enums.SaveFileType.Exchange ||
                  saveFile.Type == Enums.SaveFileType.Versus))
            {
                await MsgBox.ShowInfo(this, "This type of save is not supported yet.");
                return;
            }

            saveFile.Compress();
            await MsgBox.ShowInfo(this, "Done! Compressed save written to the original filename, with _dec removed if applicable.");
        }

        #endregion
    }
}
