﻿using System.Windows;

namespace BackupSoftware
{
	 class WindowViewModel : ViewModelBase
	 {
		  /// <summary>
		  /// The window handler
		  /// </summary>
		  private Window m_Window;

		  /// <summary>
		  /// The minimum height of the window
		  /// </summary>
		  public int WindowMinHeight { get; set; } = 400;

		  /// <summary>
		  /// The minimum width of the window
		  /// </summary>
		  public int WindowMinWidth { get; set; } = 400;

		  private int m_resizeBorder { get; set; } = 10;

		  public Thickness ResizeBorderThicknesss
		  {
			   get
			   {
					return new Thickness(m_resizeBorder);
			   }
		  }

		  public int CaptionHeight { get; set; } = 50;

		  public WindowViewModel(Window window)
		  {
			   this.m_Window = window;
			   MinimizeCommand = new RelayCommand(MinimizeWindow);
			   CloseCommand = new RelayCommand(m_Window.Close);
		  }

		  #region Commands

		  public RelayCommand MinimizeCommand { get; set; }
		  public RelayCommand CloseCommand { get; set; }

		  public void MinimizeWindow()
		  {
			   m_Window.WindowState = WindowState.Minimized;
		  }

		  #endregion
	 }
}