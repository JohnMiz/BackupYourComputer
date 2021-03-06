﻿using BackupSoftware.Core;
using BackupSoftware.Core.Services;
using BackupSoftware.Services;
using System.Windows;
using System.Windows.Input;

namespace BackupSoftware
{
	 /// <summary>
	 /// The pages types
	 /// </summary>
	 public enum ApplicationPage
	 {
		  /// <summary>
		  /// The backup data form page
		  /// </summary>
		  BackupDetailsForm = 0,

		  /// <summary>
		  /// The screen shots page
		  /// </summary>
		  ScreenshotsDetailsForm = 1,

		  /// <summary>
		  /// The select folders to backup page
		  /// </summary>
		  SelectFolders = 2,

		  /// <summary>
		  /// The select folders to backup page
		  /// </summary>
		  BackupDisplay = 3,
	 }

	 public class WindowViewModel : ViewModelBase
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

		  public int ResizeBorder { get; set; } = 0;

		  public Thickness ResizeBorderThickness
		  {
			   get
			   {
					return new Thickness(ResizeBorder + OuterMarginSize);
			   }
		  }

		  public int OuterMarginSize { get; set; } = 1;

		  public Thickness OuterMarginSizeThickness
		  {
			   get
			   {
					return new Thickness(OuterMarginSize);
			   }
		  }


		  public int CaptionHeight { get; set; } = 40;

		  public GridLength CaptionHeightGridLength { get { return new GridLength(CaptionHeight + ResizeBorder); } }

		  public WindowViewModel(Window window)
		  {
			   this.m_Window = window;
			   MinimizeCommand = new RelayCommand(MinimizeWindow);
			   CloseCommand = new RelayCommand(CloseWindow);
			   MenuCommand = new RelayCommand(() => SystemCommands.ShowSystemMenu(m_Window, GetMousePosition()));
		  }

		  /// <summary>
		  /// Get mouse position 
		  /// </summary>
		  /// <returns></returns>
		  private Point GetMousePosition()
		  {
			   // Position of the mouse relative to window
			   Point p = Mouse.GetPosition(m_Window);

			   // Add the window position so it is in the window area
			   return new Point(p.X + m_Window.Left, p.Y + m_Window.Top);
		  }

		  #region Commands

		  public ICommand MinimizeCommand { get; set; }
		  public ICommand CloseCommand { get; set; }
		  public ICommand MenuCommand { get; set; }

		  public void MinimizeWindow()
		  {
			   m_Window.WindowState = WindowState.Minimized;
		  }

		  public void CloseWindow()
		  {
			   if (!ViewModelLocator.CacheViewModel.IsBackupRunning)
			   {
					m_Window.Close();
			   }
			   else
			   {
					IDialogService dialogService = new WindowsDialogService();

					dialogService.ShowMessageBox("Please wait until the backup is finished!");
			   }

		  }

		  #endregion
	 }
}
