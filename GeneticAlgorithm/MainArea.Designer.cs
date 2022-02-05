namespace GeneticAlgorithm
{
    partial class MainArea
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
            this.LogsWindow = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // LogsWindow
            // 
            this.LogsWindow.Location = new System.Drawing.Point(12, 409);
            this.LogsWindow.Name = "LogsWindow";
            this.LogsWindow.Size = new System.Drawing.Size(860, 40);
            this.LogsWindow.TabIndex = 0;
            this.LogsWindow.Text = "";
            // 
            // MainArea
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 461);
            this.Controls.Add(this.LogsWindow);
            this.Name = "MainArea";
            this.Text = "Form1";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.PaintInd);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox LogsWindow;
    }
}

