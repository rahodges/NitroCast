using System;
using System.Collections;
using System.Windows.Forms;
using NitroCast.Core;

namespace NitroCast.Core.UI
{
    // Create a node sorter that implements the IComparer interface.
    public class NodeSorter : IComparer
    {
        // Compare the length of the strings, or the strings
        // themselves, if they are the same length.
        public int Compare(object x, object y)
        {
            TreeNode tx = (TreeNode)x;
            TreeNode ty = (TreeNode)y;

            //// Compare the length of the strings, returning the difference.
            //if (tx.Text.Length != ty.Text.Length)
            //    return tx.Text.Length - ty.Text.Length;

            // If they are the same length, call Compare.
            return string.Compare(ty.Text, tx.Text);
        }
    }

}
