namespace Gaming_2048
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.PanelBoard = new System.Windows.Forms.Panel();
            this.TimerWatch = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // PanelBoard
            // 
            this.PanelBoard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(184)))), ((int)(((byte)(175)))), ((int)(((byte)(158)))));
            this.PanelBoard.Location = new System.Drawing.Point(49, 92);
            this.PanelBoard.Name = "PanelBoard";
            this.PanelBoard.Size = new System.Drawing.Size(307, 307);
            this.PanelBoard.TabIndex = 0;
            // 
            // TimerWatch
            // 
            this.TimerWatch.Interval = 16;
            this.TimerWatch.Tick += new System.EventHandler(this.TimerWatch_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 488);
            this.Controls.Add(this.PanelBoard);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PanelBoard;
        private System.Windows.Forms.Timer TimerWatch;
    }
}

