﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace DataToolsGrasshopper.Utils
{
    /// <summary>
    /// A general static class to handle custom comparisons between objects.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal static class Compare<T> where T : IGH_Goo
    {
        /// <summary>
        /// Returns true if both DataTrees have the same number of branches and path names. 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        internal static bool EqualDataTreeStructure(GH_Structure<T> A, GH_Structure<T> B)
        {
            // Compare number of branches
            if (A.Branches.Count != B.Branches.Count) return false;

            // Compare paths one by one
            // Iterate backwards for faster out (new elements are usually added at the end)
            var pA = A.Paths;
            var pB = B.Paths;
            for (int i = pA.Count - 1; i >= 0; i--)
            {
                if (pA[i] != pB[i]) return false;
            }

            return true;
        }

        /// <summary>
        /// Returns true if all branches have the same number of elements, and each element is
        /// equal/similar under epsilon tolerance. 
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="comp"></param>
        /// <param name="epsilon"></param>
        /// <returns></returns>
        internal static bool EqualDataTreeContent(GH_Structure<T> A, GH_Structure<T> B, GH_Component comp, double epsilon = 0)
        {
            // Compare number of items in each branch.
            var bA = A.Branches;
            var bB = B.Branches;
            for (int i = bA.Count - 1; i >= 0; i--)
            {
                if (bA[i].Count != bB[i].Count) return false;
            }

            // Figure out data types and branch down to a different comparison method
            try
            {
                if (typeof(T) == typeof(GH_Boolean))
                {
                    return EqualBoolData(A as GH_Structure<GH_Boolean>, B as GH_Structure<GH_Boolean>);
                }
                else if (typeof(T) == typeof(GH_Integer))
                {
                    return EqualIntData(A as GH_Structure<GH_Integer>, B as GH_Structure<GH_Integer>);
                }
            }
            catch
            {
                comp.AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Something went wrong with the data here...");
            }

            // If here, something went wring with comparison, so flag false for an update
            return false;
        }

        /// <summary>
        /// Returns true if all bool elements of the tree are equal.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        internal static bool EqualBoolData(GH_Structure<GH_Boolean> A, GH_Structure<GH_Boolean> B)
        {
            var bA = A.Branches;
            var bB = B.Branches;
            for (int i = bA.Count - 1; i >= 0; i--)
            {
                for (int j = bA[i].Count - 1; j >= 0; j--)
                {
                    if (bA[i][j].Value != bB[i][j].Value) return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns true if all Integer elements of the tree are equal.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        internal static bool EqualIntData(GH_Structure<GH_Integer> A, GH_Structure<GH_Integer> B)
        {
            var bA = A.Branches;
            var bB = B.Branches;
            for (int i = bA.Count - 1; i >= 0; i--)
            {
                for (int j = bA[i].Count - 1; j >= 0; j--)
                {
                    if (bA[i][j].Value != bB[i][j].Value) return false;
                }
            }

            return true;
        }



        
    }
}