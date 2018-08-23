﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BackupSoftware
{
    public class DisplayViewModel : ViewModelBase
    {
		  // TEMP: Instance to check the design live
		  public static DisplayViewModel Instance => new DisplayViewModel();
		  //

		  /// <summary>
		  /// A list of all the DisplayItemControlViewModel
		  /// </summary>
		  public List<DisplayItemControlViewModel> Items { get; set; }

		  /// <summary>
		  /// How many folders done with the backup
		  /// </summary>
		  private int _DoneFoldersCount { get; set; } = 0;
		  public int DoneFoldersCount
		  {
			   get
			   {
					return _DoneFoldersCount;
			   }
			   set
			   {
					if (_DoneFoldersCount == value)
						 return;

					_DoneFoldersCount = value;
					OnPropertyChanged(nameof(DoneFoldersCount));
			   }
		  }

		  /// <summary>
		  /// Notifies when the done folder count needs to be updated
		  /// </summary>
		  public Progress<int> CountProgress { get; set; }

		  /// <summary>
		  /// True if the backup is running
		  /// </summary>
		  public bool IsBackupRunning { get; set; } = false;

		  /// <summary>
		  /// True if the overall backup is done
		  /// </summary>
		  private bool _IsBackupDone;

		  public bool IsBackupDone
		  {
			   get { return _IsBackupDone; }
			   set { if (_IsBackupDone == value) return; _IsBackupDone = value; OnPropertyChanged(nameof(IsBackupDone)); }
		  }


		  #region Commands
		  /// <summary>
		  /// The command to go back to the backup page
		  /// </summary>
		  public ICommand BackToDetailsCommand { get; set; }

		  #endregion

		  /// <summary>
		  /// Start backing up all the <see cref="FolderPathsToBackup"/> to <see cref="BackupFolder"/>
		  /// </summary>
		  async void StartBackupAsync(IProgress<int> progress)
		  {
			   IsBackupRunning = true;
			   await Task.Run(() =>
			   {
					int count = 0;
					Parallel.ForEach<DisplayItemControlViewModel>(Items, async (item) =>
					{
						 await item.StartBackup();
						 if (item.BackupDone)
						 {
							  DoneFoldersCount++;
							  count++;
							  progress.Report(count);

							  if (count == Items.Count)
							  {
								   IsBackupRunning = false;
								   IsBackupDone = true;

								   Debug.WriteLine("Done!");

								   // Redircet to details page
								   GoToDetails();
							  }
						 }
					});
			   });


		  }


		  private void GoToDetails()
		  {
			   ViewModelLocator.ApplicationViewModel.GoToView(ViewModelLocator.DetailsViewModel);
		  }

		  /// <summary>
		  /// Setting all the information(DestFolder, FolderPath) to <see cref="Items"/>
		  /// </summary>
		  private void SetItemsInformation()
		  {
			   // Getting all the infomation to Items
			   foreach (var item in ViewModelLocator.CacheViewModel.Details.SourceFolders)
			   {
					DisplayItemControlViewModel displayItemControlViewModel = new DisplayItemControlViewModel(item)
					{
						 Destination = $"{ViewModelLocator.CacheViewModel.Details.DestFolder}{item.FolderInfo.Name}"
					};

					Items.Add(displayItemControlViewModel);
			   }

		  }

		  public void RunBackup()
		  {
			   if (!IsBackupRunning)
			   {
					// In the begining the backup is not yet started
					IsBackupDone = false;

					// Setting all the infomation to Items
					SetItemsInformation();

					// Start the backup
					StartBackupAsync(CountProgress);
			   }
		  }

		  public DisplayViewModel()
		  {
			   Items = new List<DisplayItemControlViewModel>();

			   // Create command
			   BackToDetailsCommand = new RelayCommand(GoToDetails);

			   CountProgress = new Progress<int>();

			   // Set the event for handling the progression
			   CountProgress.ProgressChanged += CountProgress_ProgressChanged;
		  }

		  private void CountProgress_ProgressChanged(object sender, int e)
		  {
			   DoneFoldersCount = (e * 100) / Items.Count;
		  }
	 }
}
