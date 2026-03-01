using Avalonia.Controls;
using Avalonia.Interactivity;

namespace FEFTwiddler.GUI.GlobalData
{
    public partial class HairColors : Window
    {
        private readonly Model.GlobalSave _globalSave;
        private static readonly Model.GameColor NoColor = new Model.GameColor(1, 0, 0, 0);

        public HairColors(Model.GlobalSave globalSave)
        {
            _globalSave = globalSave;
            InitializeComponent();
            PopulateControls();
        }

        private void PopulateControls()
        {
            colorCorrinM.Color = _globalSave.Region1.HairColor_CorrinM;
            colorCorrinF.Color = _globalSave.Region1.HairColor_CorrinF;
            colorKanaM.Color = _globalSave.Region1.HairColor_KanaM;
            colorKanaF.Color = _globalSave.Region1.HairColor_KanaF;
            colorShigure.Color = _globalSave.Region1.HairColor_Shigure;
            colorDwyer.Color = _globalSave.Region1.HairColor_Dwyer;
            colorSophie.Color = _globalSave.Region1.HairColor_Sophie;
            colorMidori.Color = _globalSave.Region1.HairColor_Midori;
            colorShiro.Color = _globalSave.Region1.HairColor_Shiro;
            colorKiragi.Color = _globalSave.Region1.HairColor_Kiragi;
            colorAsugi.Color = _globalSave.Region1.HairColor_Asugi;
            colorSelkie.Color = _globalSave.Region1.HairColor_Selkie;
            colorHisame.Color = _globalSave.Region1.HairColor_Hisame;
            colorMitama.Color = _globalSave.Region1.HairColor_Mitama;
            colorCaeldori.Color = _globalSave.Region1.HairColor_Caeldori;
            colorRhajat.Color = _globalSave.Region1.HairColor_Rhajat;
            colorSiegbert.Color = _globalSave.Region1.HairColor_Siegbert;
            colorForrest.Color = _globalSave.Region1.HairColor_Forrest;
            colorIgnatius.Color = _globalSave.Region1.HairColor_Ignatius;
            colorVelouria.Color = _globalSave.Region1.HairColor_Velouria;
            colorPercy.Color = _globalSave.Region1.HairColor_Percy;
            colorOphelia.Color = _globalSave.Region1.HairColor_Ophelia;
            colorSoleil.Color = _globalSave.Region1.HairColor_Soleil;
            colorNina.Color = _globalSave.Region1.HairColor_Nina;

            DisableMissingColors();
        }

        private void DisableMissingColors()
        {
            if (colorNina.Color.Equals(NoColor)) colorNina.IsEnabled = false; else return;
            if (colorSoleil.Color.Equals(NoColor)) colorSoleil.IsEnabled = false; else return;
            if (colorOphelia.Color.Equals(NoColor)) colorOphelia.IsEnabled = false; else return;
            if (colorPercy.Color.Equals(NoColor)) colorPercy.IsEnabled = false; else return;
            if (colorVelouria.Color.Equals(NoColor)) colorVelouria.IsEnabled = false; else return;
            if (colorIgnatius.Color.Equals(NoColor)) colorIgnatius.IsEnabled = false; else return;
            if (colorForrest.Color.Equals(NoColor)) colorForrest.IsEnabled = false; else return;
            if (colorSiegbert.Color.Equals(NoColor)) colorSiegbert.IsEnabled = false; else return;
            if (colorRhajat.Color.Equals(NoColor)) colorRhajat.IsEnabled = false; else return;
            if (colorCaeldori.Color.Equals(NoColor)) colorCaeldori.IsEnabled = false; else return;
            if (colorMitama.Color.Equals(NoColor)) colorMitama.IsEnabled = false; else return;
            if (colorHisame.Color.Equals(NoColor)) colorHisame.IsEnabled = false; else return;
            if (colorSelkie.Color.Equals(NoColor)) colorSelkie.IsEnabled = false; else return;
            if (colorAsugi.Color.Equals(NoColor)) colorAsugi.IsEnabled = false; else return;
            if (colorKiragi.Color.Equals(NoColor)) colorKiragi.IsEnabled = false; else return;
            if (colorShiro.Color.Equals(NoColor)) colorShiro.IsEnabled = false; else return;
            if (colorMidori.Color.Equals(NoColor)) colorMidori.IsEnabled = false; else return;
            if (colorSophie.Color.Equals(NoColor)) colorSophie.IsEnabled = false; else return;
            if (colorDwyer.Color.Equals(NoColor)) colorDwyer.IsEnabled = false; else return;
            if (colorShigure.Color.Equals(NoColor)) colorShigure.IsEnabled = false; else return;
            if (colorKanaF.Color.Equals(NoColor)) colorKanaF.IsEnabled = false; else return;
            if (colorKanaM.Color.Equals(NoColor)) colorKanaM.IsEnabled = false; else return;
        }

        private void BtnSaveAndClose_Click(object? sender, RoutedEventArgs e)
        {
            _globalSave.Region1.HairColor_CorrinM = colorCorrinM.Color;
            _globalSave.Region1.HairColor_CorrinF = colorCorrinF.Color;
            _globalSave.Region1.HairColor_KanaM = colorKanaM.Color;
            _globalSave.Region1.HairColor_KanaF = colorKanaF.Color;
            _globalSave.Region1.HairColor_Shigure = colorShigure.Color;
            _globalSave.Region1.HairColor_Dwyer = colorDwyer.Color;
            _globalSave.Region1.HairColor_Sophie = colorSophie.Color;
            _globalSave.Region1.HairColor_Midori = colorMidori.Color;
            _globalSave.Region1.HairColor_Shiro = colorShiro.Color;
            _globalSave.Region1.HairColor_Kiragi = colorKiragi.Color;
            _globalSave.Region1.HairColor_Asugi = colorAsugi.Color;
            _globalSave.Region1.HairColor_Selkie = colorSelkie.Color;
            _globalSave.Region1.HairColor_Hisame = colorHisame.Color;
            _globalSave.Region1.HairColor_Mitama = colorMitama.Color;
            _globalSave.Region1.HairColor_Caeldori = colorCaeldori.Color;
            _globalSave.Region1.HairColor_Rhajat = colorRhajat.Color;
            _globalSave.Region1.HairColor_Siegbert = colorSiegbert.Color;
            _globalSave.Region1.HairColor_Forrest = colorForrest.Color;
            _globalSave.Region1.HairColor_Ignatius = colorIgnatius.Color;
            _globalSave.Region1.HairColor_Velouria = colorVelouria.Color;
            _globalSave.Region1.HairColor_Percy = colorPercy.Color;
            _globalSave.Region1.HairColor_Ophelia = colorOphelia.Color;
            _globalSave.Region1.HairColor_Soleil = colorSoleil.Color;
            _globalSave.Region1.HairColor_Nina = colorNina.Color;

            Close();
        }

        private void BtnCancel_Click(object? sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
