﻿using BackupSoftware.Services;
using Microsoft.WindowsAPICodePack.Dialogs;
using Ninject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BackupSoftware
{
	 public class DetailsViewModel : ViewModelBase
	 {

		  #region Private Members

		  /// <summary>
		  /// Dialog service to display messages to the screen
		  /// </summary>
		  private IDialogService _DialogService;

		  /// <summary>
		  /// The view model of the display view page
		  /// </summary>
		  private DisplayViewModel _DisplayViewModel;
		  public DisplayViewModel DisplayViewModel
		  {
			   get { return _DisplayViewModel; }
			   set { if (_DisplayViewModel == value) return; _DisplayViewModel = value; OnPropertyChanged(nameof(DisplayViewModel)); }
		  }
		  /// <summary>
		  /// The view model of the display view page
		  /// </summary>
		  private SelectSourceViewModel _SelectSourceViewModel;
		  public SelectSourceViewModel SelectSourceViewModel
		  {
			   get { return _SelectSourceViewModel; }
			   set { if (_SelectSourceViewModel == value) return; _SelectSourceViewModel = value; OnPropertyChanged(nameof(SelectSourceViewModel)); }
		  }


		  #endregion

		  #region Public Members


		  /// <summary>
		  /// Reference to <see cref="CacheViewModel.Details"/>
		  /// </summary>
		  public Details Details
		  {
			   get
			   {
					return ViewModelLocator.CacheViewModel.Details;
			   }
			   set
			   {
					ViewModelLocator.CacheViewModel.Details = value;
			   }
		  }


		  public string LastBackupTimeText
		  {
			   get { if (Details.LastBackupTime == new DateTime()) return "No backup history."; else return Details.LastBackupTime.ToString("HH:mm:ss dd/MM/yyyy"); }
		  }

		  #endregion

		  #region Helpers

		  #endregion

		  #region Commands

		  /// <summary>
		  /// The command to choose destination folder
		  /// </summary>
		  public ICommand SelectDestFolderCommand { get; set; }
		  /// <summary>
		  /// The command to start the backup
		  /// </summary>
		  public ICommand StartBackupCommand { get; set; }
		  /// <summary>
		  /// The command to open the page of selecting folders to backup
		  /// </summary>
		  public ICommand GoToSelectSourceCommand { get; set; }

		  /// <summary>
		  /// The command to redirect to the display view
		  /// </summary>
		  public ICommand GoToDisplayCommand { get; set; }
		  #endregion

		  #region Command Functions

		  /// <summary>
		  /// The action when the user choose backup folder
		  /// </summary>
		  private void SelectDestFolder()
		  {
			   // Reset the state to the initial values
			   if (DisplayViewModel.IsBackupDone && !DisplayViewModel.IsBackupRunning)
			   {
					DisplayViewModel.ResetState();
			   }

			   string folder = _DialogService.SelectFolder("Choose folder to backup in hard drive");

			   if (folder != null)
					Details.DestFolder = folder;

		  }

		  /// <summary>
		  /// Redirecting the user to the select source view page
		  /// </summary>
		  private void GoToSelectSource()
		  {

			   // Reset the state to the initial values
			   if (DisplayViewModel.IsBackupDone && !DisplayViewModel.IsBackupRunning)
			   {
					DisplayViewModel.ResetState();
			   }

			   // Injecting the SelectSourceViewModel
			   ViewModelLocator.ApplicationViewModel.GoToView(_SelectSourceViewModel);
		  }

		  /// <summary>
		  /// Go to the display view
		  /// </summary>
		  private void GoToDisplay()
		  {
			   if (DisplayViewModel.IsBackupRunning)
			   {
					ViewModelLocator.ApplicationViewModel.GoToView(DisplayViewModel);
			   }
		  }
		  
		  /// <summary>
		  /// Validates if the user gave right input
		  /// </summary>
		  private bool ValidateUserInput(Details details)
		  {
			   // Checks to see if there is content in the fields
			   if (string.IsNullOrEmpty(details.DestFolder) || details.SourceFolders.Count == 0)
			   {
					_DialogService.ShowMessageBox("Fill in the list of folder and hard drive!");
					return false;
			   }

			   // Check to see if the folders exists
			   foreach (SourceFolder item in details.SourceFolders)
			   {
					if (!Directory.Exists(item.FolderInfo.FullPath))
					{
						 _DialogService.ShowMessageBox(item.FolderInfo.FullPath + " can not be found!");
						 return false;
					}

			   }

			   if (!Directory.Exists(details.DestFolder))
			   {
					_DialogService.ShowMessageBox(details.DestFolder + " can not be found!");
					return false;
			   }

			   return true;
		  }

		  private void StartBackup()
		  {
			   if (!DisplayViewModel.IsBackupRunning)
			   {
					if (_DialogService.ShowYesNoMessageBox("Are you sure you want to start the backup?", "Question"))
					{
						 // Validate user input
						 if (ValidateUserInput(Details))
						 {
							  // Run the back up
							  DisplayViewModel.RunBackup();

							  // Redirect the user to the display view page
							  GoToDisplay();
						 }
					}
			   }
			   else
			   {
					_DialogService.ShowMessageBox("The buckup is already running!");
			   }
		  }
		  #endregion

		  /// <summary>
		  /// Default Constructor
		  /// </summary>
		  public DetailsViewModel(IDialogService dialogService, SelectSourceViewModel selectSourceViewModel,  DisplayViewModel displayViewModel)
		  {
			   _DialogService = dialogService;
			   DisplayViewModel = displayViewModel;
			   SelectSourceViewModel = selectSourceViewModel;


			   SelectDestFolderCommand = new RelayCommand(SelectDestFolder, (parameter)=> { return !DisplayViewModel.IsBackupRunning; });
			   GoToSelectSourceCommand = new RelayCommand(GoToSelectSource, (parameter) => { return !DisplayViewModel.IsBackupRunning; });
			   StartBackupCommand = new RelayCommand(StartBackup);
			   GoToDisplayCommand = new RelayCommand(GoToDisplay);
		  }
	 }
}
