using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnimeStudio.GUI
{
    partial class GameSelector : Form
    {
        static readonly Dictionary<GameType, string> GameDisplayNames = new()
        {
            { GameType.Normal, "Unity" },
            { GameType.UnityCN, "Unity CN" },
            { GameType.GI, "Live" },
            { GameType.GI_Pack, "Pack" },
            { GameType.GI_CB1, "CBT 1" },
            { GameType.GI_CB2, "CBT 2" },
            { GameType.GI_CB3, "CBT 3" },
            { GameType.GI_CB3Pre, "CBT 3 Pre" },
            { GameType.BH3, "Live" },
            { GameType.BH3Pre, "Pre" },
            { GameType.BH3PrePre, "Pre Pre" },
            { GameType.SR, "Live" },
            { GameType.SR_CB2, "CBT 2" },
            { GameType.ZZZ, "Live" },
            { GameType.ZZZ_CB1, "CBT 1" },
            { GameType.ZZZ_CB2, "CBT 2" },
            { GameType.TOT, "Live" },
            { GameType.Naraka, "Naraka" },
            { GameType.EnsembleStars, "Ensemble Stars" },
            { GameType.OPFP, "OPFP" },
            { GameType.FakeHeader, "Fake Header" },
            { GameType.FantasyOfWind, "Fantasy of Wind" },
            { GameType.ShiningNikki, "Shining Nikky" },
            { GameType.HelixWaltz2, "Helix Waltz 2" },
            { GameType.NetEase, "Net Ease" },
            { GameType.AnchorPanic, "Anchor Panic" },
            { GameType.DreamscapeAlbireo, "Dreamscape Albireo" },
            { GameType.ImaginaryFest, "Imaginary Fest" },
            { GameType.AliceGearAegis, "Alice Gear Aegis" },
            { GameType.ProjectSekai, "Project Sekai" },
            { GameType.CodenameJump, "Codename Jump" },
            { GameType.GirlsFrontline, "Girls Frontline" },
            { GameType.Reverse1999, "Reverse: 1999" },
            { GameType.ArknightsEndfield, "Arknights Endfield" },
            { GameType.JJKPhantomParade, "JJK Phantom Parade" },
            { GameType.MuvLuvDimensions, "Muv Luv Dimensions" },
            { GameType.PartyAnimals, "Party Animals" },
            { GameType.LoveAndDeepspace, "Love and Deepspace" },
            { GameType.SchoolGirlStrikers, "School Girl Strikers" },
            { GameType.ExAstris, "Ex Astris" },
            { GameType.PerpetualNovelty, "Perpetual Novelty" },
        };

        static readonly List<GameType[]> HoyoGames = new List<GameType[]>
        {
            new[] { GameType.GI, GameType.GI_Pack, GameType.GI_CB1, GameType.GI_CB2, GameType.GI_CB3, GameType.GI_CB3Pre },
            new[] { GameType.BH3, GameType.BH3Pre, GameType.BH3PrePre },
            new[] { GameType.SR, GameType.SR_CB2 },
            new[] { GameType.ZZZ, GameType.ZZZ_CB1, GameType.ZZZ_CB2 },
            new[] { GameType.TOT },
        };

        static readonly GameType[] UnityGames = new[]
        {
            GameType.Normal,
            GameType.UnityCN,
            GameType.FakeHeader,
        };

        static readonly GameType[] OtherGames = new[]
        {
            GameType.Naraka,
            GameType.EnsembleStars,
            GameType.OPFP,
            GameType.FantasyOfWind,
            GameType.ShiningNikki,
            GameType.HelixWaltz2,
            GameType.NetEase,
            GameType.AnchorPanic,
            GameType.DreamscapeAlbireo,
            GameType.ImaginaryFest,
            GameType.AliceGearAegis,
            GameType.ProjectSekai,
            GameType.CodenameJump,
            GameType.GirlsFrontline,
            GameType.Reverse1999,
            GameType.ArknightsEndfield,
            GameType.JJKPhantomParade,
            GameType.MuvLuvDimensions,
            GameType.PartyAnimals,
            GameType.LoveAndDeepspace,
            GameType.SchoolGirlStrikers,
            GameType.ExAstris,
            GameType.PerpetualNovelty,
        };

        private GameType selectedGame;
        private readonly MainForm _parent;

        public GameSelector(MainForm parent)
        {
            InitializeComponent();
            _parent = parent;
        }

        private GameType GetGameTypeFromComboBox(string selectedName)
        {
            return GameDisplayNames.FirstOrDefault(x => x.Value == selectedName).Key;
        }

        private void gameTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            gameCombo.Items.Clear();
            hoyoCombo.Enabled = false;
            hoyoCombo.Items.Clear();

            switch (gameTypeCombo.SelectedIndex)
            {
                case 0:
                    gameCombo.Items.AddRange(["Genshin Impact", "Honkai Impact 3rd", "Honkai: Star Rail", "Zenless Zone Zero", "Tears of Themis"]);
                    break;
                case 1:
                    gameCombo.Items.AddRange(OtherGames.Select(x => GameDisplayNames[x]).ToArray());
                    break;
                case 2:
                    gameCombo.Items.AddRange(UnityGames.Select(x => GameDisplayNames[x]).ToArray());
                    break;
            };

            gameCombo.Enabled = true;
        }

        private void gameCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (gameTypeCombo.SelectedIndex != 0)
            {
                selectedGame = GetGameTypeFromComboBox(gameCombo.SelectedItem.ToString());
            }
            else
            {
                hoyoCombo.Items.Clear();
                hoyoCombo.Items.AddRange(HoyoGames[gameCombo.SelectedIndex].Select(x => GameDisplayNames[x]).ToArray());
                hoyoCombo.Enabled = true;
            }
        }

        private void hoyoCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedGame = HoyoGames[gameCombo.SelectedIndex][hoyoCombo.SelectedIndex];
        }

        private void confirmBtn_Click(object sender, EventArgs e)
        {
            _parent.updateGame((int)selectedGame);
            this.Close();
        }
    }
}
