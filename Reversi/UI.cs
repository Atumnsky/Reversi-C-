namespace Reversi
{
    public class UI
    {
        public Panel UIPanel;
        public Panel PPanel;

        public Button NewGameButton;
        public Button HintButton;
        public Button VSAIButton;
        public Button HelpButton;
        public ComboBox FieldSizeBox;
        public ComboBox ColorBox;
        public ComboBox DifficultyBox;

        public EventHandler NewGameClicked;
        public EventHandler FieldSizeChanged;
        public EventHandler ColorChanged;
        public EventHandler HintClicked;
        public EventHandler AIClicked;
        public EventHandler HelpClicked;
        public EventHandler DifficultyChanged;

        Point mouse;
        public bool HintOn = false;
        public bool VSAIOn = false;

        public UI(Font font1, Font font2, Size ppSize)
        {
            UIPanel = new Panel();
            UIPanel.Size = new Size(120 * 6, Settings.Uheight);

            PPanel = new Panel();
            PPanel.Size = ppSize;

            NewGameButton = new Button();
            NewGameButton.Text = "New Game";
            NewGameButton.Size = new Size(120, UIPanel.Size.Height / 3);
            NewGameButton.Location = new Point(0, 0);
            NewGameButton.Font = font1;
            NewGameButton.Click += OnNewGameClicked;

            int space = NewGameButton.Size.Width / 4;

            VSAIButton = new Button();
            VSAIButton.Text = "VS Robot";
            VSAIButton.Font = font1;
            VSAIButton.Size = NewGameButton.Size;
            VSAIButton.Location = new Point(NewGameButton.Location.X + NewGameButton.Width + space, 0);
            VSAIButton.Click += ONAIClicked;

            DifficultyBox = new ComboBox();
            DifficultyBox.Location = new Point(NewGameButton.Location.X + NewGameButton.Width + space, VSAIButton.Height + 1);
            DifficultyBox.Font = font2;
            DifficultyBox.DropDownStyle = ComboBoxStyle.DropDownList;

            DifficultyBox.Items.Add("Easy");
            DifficultyBox.Items.Add("Medium");
            DifficultyBox.Items.Add("Hard");

            DifficultyBox.SelectedIndexChanged += OnDifficultyChanged;

            HintButton = new Button();
            HintButton.Text = "Move Hints";
            HintButton.Font = font1;
            HintButton.Size = NewGameButton.Size;
            HintButton.Location = new Point(VSAIButton.Location.X + VSAIButton.Width + space, 0);
            HintButton.Click += OnHintClicked;

            HelpButton = new Button();
            HelpButton.Text = "Help";
            HelpButton.Font = font1;
            HelpButton.Size = new Size(100, 30);
            HelpButton.FlatStyle = FlatStyle.Flat;
            HelpButton.FlatAppearance.BorderSize = 0;
            HelpButton.Location = new Point(PPanel.Size.Width - 110, PPanel.Size.Height - 40);
            HelpButton.Click += OnHelpClicked;

            FieldSizeBox = new ComboBox();
            FieldSizeBox.Location = new Point(HintButton.Location.X + HintButton.Width + space, 0);
            FieldSizeBox.Font = font2;
            FieldSizeBox.DropDownStyle = ComboBoxStyle.DropDownList;

            FieldSizeBox.Items.Add("4x4");
            FieldSizeBox.Items.Add("6x6");
            FieldSizeBox.Items.Add("8x8");
            FieldSizeBox.Items.Add("10x10");

            FieldSizeBox.SelectedIndexChanged += OnFieldSizeChanged;

            ColorBox = new ComboBox();
            ColorBox.Location = new Point(FieldSizeBox.Location.X + FieldSizeBox.Width + space, 0);
            ColorBox.Font = font2;
            ColorBox.DropDownStyle = ComboBoxStyle.DropDownList;

            ColorBox.Items.Add("Classic Board");
            ColorBox.Items.Add("Red & Blue");
            ColorBox.Items.Add("Wood");

            ColorBox.SelectedIndexChanged += OnColorChanged;

            void OnColorChanged(object sender, EventArgs e)
            {
                if (ColorChanged != null)
                    ColorChanged(sender, e);
            }

            void OnHintClicked(object s, EventArgs ea)
            {
                HintOn = !HintOn;
                if (HintOn)
                    HintButton.BackColor = Color.LimeGreen;
                else
                    HintButton.BackColor = Color.White;

                if (HintClicked != null)
                    HintClicked(s, ea);
            }

            void ONAIClicked(object s, EventArgs ea)
            {
                VSAIOn = !VSAIOn;

                if (VSAIOn)
                    VSAIButton.BackColor = Color.Orange;
                else
                    VSAIButton.BackColor = Color.White;

                if (AIClicked != null)
                    AIClicked(s, ea);
            }

            UIPanel.Controls.Add(NewGameButton);
            UIPanel.Controls.Add(FieldSizeBox);
            UIPanel.Controls.Add(ColorBox);
            UIPanel.Controls.Add(HintButton);
            UIPanel.Controls.Add(VSAIButton);
            UIPanel.Controls.Add(DifficultyBox);

            PPanel.Controls.Add(HelpButton);

            void OnHelpClicked(object sender, EventArgs e)
            {
                if (HelpClicked != null)
                    HelpClicked(sender, e);
            }

            void OnNewGameClicked(object sender, EventArgs e)
            {
                if (NewGameClicked != null)
                    NewGameClicked(sender, e);
            }

            void OnFieldSizeChanged(object sender, EventArgs e)
            {
                if (FieldSizeChanged != null)
                    FieldSizeChanged(sender, e);
            }

            void OnDifficultyChanged(object sender, EventArgs e)
            {
                if (DifficultyChanged != null)
                    DifficultyChanged(sender, e);
            }
        }
    }
}
