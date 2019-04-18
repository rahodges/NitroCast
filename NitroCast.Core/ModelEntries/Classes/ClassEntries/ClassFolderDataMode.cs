using System;

namespace NitroCast.Core
{
	/// <summary>
	/// Sets the folder data mode, which can be Integrated, Isolated, Partitioned or PartitionedReadAhead.
	/// Integrated mode loads the folder with the default load method.
	/// Isolated mode ignores the folder with default load method unless the folder is
	/// specifically used as a parameter to the overridden load method. It will not, however,
	/// save the modified data in the isolated folder unless it is specified in the overridden
	/// save method.
	/// Paritioned mode ingores the folder with the default load method unless the folder is
	/// specifically used as a parameter to the overrident load method. It will save modified
	/// data in the partition automatically with default save method.
	/// PartionedReadAhead mode acts just as the partitioned mode except it loads the partition's
	/// data along with the default load method to save time.
	/// </summary>
	public enum ClassFolderDataMode
	{
		/// <summary>
		/// Specifies that the class folder will be intergrated fully
		/// with the class. It will load and save fields in the folder
		/// with the default save and load methods.
		/// <code>Person.Load();								// Loads all integrated and partitioned read ahead folders
		/// Person.Address1 = "5519 East Saint Martin's St.";	// Changes address
		/// Person.Save()</code>								// Saves all integrated and changed partitions
		/// </summary>
		Integrated,

		/// <summary>
		/// The class folder's objects will only be loaded and saved
		/// to the database when the folder is specifically saved through
		/// the save method overrides.
		/// <code>Person.Load(ClassObjectFolders.Address);		// ... forces load of the Address Partition folder
		/// Person.Address1 = "5199 East Saint Martin's St.";	// Change address
		/// // Person.Save();									// This will NOT persist the address change
		/// Person.Save(ClassObjectFolders.Address);			// Saves the Address Folder only
		/// </code>
		/// </summary>
		Isolated,

		/// <summary>
		/// The class folder's objects will not be loaded from the
		/// database unless specified in the load method. If the
		/// folder's objects change, they will persist to the database
		/// automatically on save.
		/// <code>Person.Load();								// ... will not load the Address Partition folder
		/// WelcomePanel.Name = Person.Name;					// Diplay person's name
		/// 
		/// Person.Address1 = "5199 East Saint Martin's St.";	// Triggers a database read to get Address Folder objects first
		/// Person.Save();										// This saves Address Folder only
		/// </code>
		/// </summary>
		Partitioned,

		/// <summary>
		/// The class folder's objects will be loaded from the database,
		/// however will not save information in the partitioned folder. This
		/// feature is excellent for loading information and skipping
		/// updates on the folder's objects if they have not been changed.
		/// <code>Person.Load();								// ... will load the Address Partition folder
		/// ContactCard.Address1.Text = Person.Address1;		// A nice address
		/// Person.Save();										// Nothing is persisted, no change detected
		/// 
		/// Person.Name = "Mark Jameson";						// Change name
		/// Person.Save();										// Persists all integrated folders
		/// 
		/// Person.Address1 = "5199 East Saint Martin's St.";	// Change address
		/// Person.Save();										// This saves the Address Folder only
		/// </code>
		/// </summary>
		PartitionedReadAhead
	}
}
