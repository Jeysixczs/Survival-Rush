namespace Shooting_Game
{
    partial class MainMenuForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainMenuForm));
            this.pictureBoxStart = new System.Windows.Forms.PictureBox();
            this.btnTwoPlayer = new System.Windows.Forms.PictureBox();
            this.btnSinglePlayer = new System.Windows.Forms.PictureBox();
            this.btnDifficultyHard = new System.Windows.Forms.PictureBox();
            this.btnDifficultyEasy = new System.Windows.Forms.PictureBox();
            this.btnDifficultyMedium = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnTwoPlayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSinglePlayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDifficultyHard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDifficultyEasy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDifficultyMedium)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxStart
            // 
            this.pictureBoxStart.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBoxStart.BackgroundImage")));
            this.pictureBoxStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBoxStart.Location = new System.Drawing.Point(778, 761);
            this.pictureBoxStart.Margin = new System.Windows.Forms.Padding(21, 14, 21, 14);
            this.pictureBoxStart.Name = "pictureBoxStart";
            this.pictureBoxStart.Size = new System.Drawing.Size(380, 92);
            this.pictureBoxStart.TabIndex = 16;
            this.pictureBoxStart.TabStop = false;
            this.pictureBoxStart.Click += new System.EventHandler(this.pictureBoxStart_Click);
            // 
            // btnTwoPlayer
            // 
            this.btnTwoPlayer.BackColor = System.Drawing.Color.Transparent;
            this.btnTwoPlayer.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnTwoPlayer.BackgroundImage")));
            this.btnTwoPlayer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnTwoPlayer.Location = new System.Drawing.Point(1002, 180);
            this.btnTwoPlayer.Margin = new System.Windows.Forms.Padding(21, 14, 21, 14);
            this.btnTwoPlayer.Name = "btnTwoPlayer";
            this.btnTwoPlayer.Size = new System.Drawing.Size(607, 431);
            this.btnTwoPlayer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.btnTwoPlayer.TabIndex = 14;
            this.btnTwoPlayer.TabStop = false;
            this.btnTwoPlayer.Click += new System.EventHandler(this.btnTwoPlayer_Click);
            // 
            // btnSinglePlayer
            // 
            this.btnSinglePlayer.BackColor = System.Drawing.Color.Transparent;
            this.btnSinglePlayer.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSinglePlayer.BackgroundImage")));
            this.btnSinglePlayer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSinglePlayer.Location = new System.Drawing.Point(327, 199);
            this.btnSinglePlayer.Margin = new System.Windows.Forms.Padding(21, 14, 21, 14);
            this.btnSinglePlayer.Name = "btnSinglePlayer";
            this.btnSinglePlayer.Size = new System.Drawing.Size(584, 391);
            this.btnSinglePlayer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnSinglePlayer.TabIndex = 15;
            this.btnSinglePlayer.TabStop = false;
            this.btnSinglePlayer.Click += new System.EventHandler(this.btnSinglePlayer_Click);
            // 
            // btnDifficultyHard
            // 
            this.btnDifficultyHard.BackColor = System.Drawing.Color.Transparent;
            this.btnDifficultyHard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDifficultyHard.Image = global::Shooting_Game.Properties.Resources.btnhardicon;
            this.btnDifficultyHard.Location = new System.Drawing.Point(1186, 637);
            this.btnDifficultyHard.Margin = new System.Windows.Forms.Padding(16, 2, 16, 2);
            this.btnDifficultyHard.Name = "btnDifficultyHard";
            this.btnDifficultyHard.Size = new System.Drawing.Size(349, 92);
            this.btnDifficultyHard.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnDifficultyHard.TabIndex = 11;
            this.btnDifficultyHard.TabStop = false;
            this.btnDifficultyHard.Click += new System.EventHandler(this.btnDifficultyHard_Click);
            // 
            // btnDifficultyEasy
            // 
            this.btnDifficultyEasy.BackColor = System.Drawing.Color.Transparent;
            this.btnDifficultyEasy.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDifficultyEasy.Image = global::Shooting_Game.Properties.Resources.btneasyicon;
            this.btnDifficultyEasy.Location = new System.Drawing.Point(366, 637);
            this.btnDifficultyEasy.Margin = new System.Windows.Forms.Padding(16, 2, 16, 2);
            this.btnDifficultyEasy.Name = "btnDifficultyEasy";
            this.btnDifficultyEasy.Size = new System.Drawing.Size(349, 92);
            this.btnDifficultyEasy.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnDifficultyEasy.TabIndex = 12;
            this.btnDifficultyEasy.TabStop = false;
            this.btnDifficultyEasy.Click += new System.EventHandler(this.btnDifficultyEasy_Click);
            // 
            // btnDifficultyMedium
            // 
            this.btnDifficultyMedium.BackColor = System.Drawing.Color.Transparent;
            this.btnDifficultyMedium.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnDifficultyMedium.Image = global::Shooting_Game.Properties.Resources.btnmediumicon;
            this.btnDifficultyMedium.Location = new System.Drawing.Point(793, 637);
            this.btnDifficultyMedium.Margin = new System.Windows.Forms.Padding(16, 2, 16, 2);
            this.btnDifficultyMedium.Name = "btnDifficultyMedium";
            this.btnDifficultyMedium.Size = new System.Drawing.Size(349, 92);
            this.btnDifficultyMedium.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.btnDifficultyMedium.TabIndex = 13;
            this.btnDifficultyMedium.TabStop = false;
            this.btnDifficultyMedium.Click += new System.EventHandler(this.btnDifficultyMedium_Click);
            // 
            // MainMenuForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = global::Shooting_Game.Properties.Resources.MainMenu;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1951, 1068);
            this.Controls.Add(this.pictureBoxStart);
            this.Controls.Add(this.btnTwoPlayer);
            this.Controls.Add(this.btnSinglePlayer);
            this.Controls.Add(this.btnDifficultyHard);
            this.Controls.Add(this.btnDifficultyEasy);
            this.Controls.Add(this.btnDifficultyMedium);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "MainMenuForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnTwoPlayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSinglePlayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDifficultyHard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDifficultyEasy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDifficultyMedium)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBoxStart;
        private System.Windows.Forms.PictureBox btnTwoPlayer;
        private System.Windows.Forms.PictureBox btnSinglePlayer;
        private System.Windows.Forms.PictureBox btnDifficultyHard;
        private System.Windows.Forms.PictureBox btnDifficultyEasy;
        private System.Windows.Forms.PictureBox btnDifficultyMedium;
    }
}

