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
        #region Games
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

        static readonly GameType[] UnityGames = GameManager.GetGames().Where(x => x.Category == GameCategory.Unity).Select(x => x.Type).ToArray();

        static readonly GameType[] OtherGames = GameManager.GetGames().Where(x => x.Category == GameCategory.Other).Select(x => x.Type).ToArray();
        #endregion

        private List<Game> sortedGames;
        private GameType selectedGame;
        private readonly MainForm _parent;

        public GameSelector(MainForm parent)
        {
            InitializeComponent();
            _parent = parent;

            sortedGames = OtherGames
            .Select(x => GameManager.GetGame(x))
            .OrderBy(g => g.DisplayName)
            .ToList();
        }

        private void gameTypeCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            gameCombo.Items.Clear();
            hoyoCombo.Enabled = false;
            hoyoCombo.Items.Clear();

            customKeyText.Enabled = false;
            customKeyText.Text = "";

            switch (gameTypeCombo.SelectedIndex)
            {
                case 0:
                    gameCombo.Items.AddRange(["Genshin Impact", "Honkai Impact 3rd", "Honkai: Star Rail", "Zenless Zone Zero", "Tears of Themis"]);
                    break;
                case 1:
                    gameCombo.Items.AddRange(sortedGames.Select(g => g.DisplayName).ToArray());
                    break;
                case 2:
                    gameCombo.Items.AddRange(UnityGames.Select(x => GameManager.GetGame(x).DisplayName).ToArray());
                    break;
            };

            gameCombo.Enabled = true;
        }

        private void gameCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            customKeyText.Enabled = false;
            customKeyText.Text = "";

            switch (gameTypeCombo.SelectedIndex)
            {
                case 0:
                    hoyoCombo.Items.Clear();
                    hoyoCombo.Items.AddRange(HoyoGames[gameCombo.SelectedIndex].Select(x => GameDisplayNames[x]).ToArray());
                    hoyoCombo.SelectedIndex = 0;
                    hoyoCombo.Enabled = true;
                    break;
                case 1:
                    selectedGame = sortedGames[gameCombo.SelectedIndex].Type;
                    break;
                case 2:
                    selectedGame = UnityGames[gameCombo.SelectedIndex];
                    if (selectedGame == GameType.UnityCNCustomKey)
                    {
                        customKeyText.Enabled = true;
                        customKeyText.Text = "";
                    }
                    break;
            }
        }

        private void hoyoCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedGame = HoyoGames[gameCombo.SelectedIndex][hoyoCombo.SelectedIndex];
        }

        private void confirmBtn_Click(object sender, EventArgs e)
        {
            if (selectedGame is GameType.UnityCNCustomKey)
            {
                Game gameObject = GameManager.GetGame(selectedGame);
                if (gameObject is UnityCNGame unityCNGame)
                {
                    unityCNGame.Key.Key = customKeyText.Text;
                }
            }
            _parent.updateGame((int)selectedGame);
            this.Close();
        }
    }
}
