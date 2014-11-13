////****************************************************************************
// <copyright file="Utility.cs" company="Wilbur-Ellis">
// Copyright (c) Wilbur-Ellis. All rights reserved.
// </copyright>
////****************************************************************************

using System;

namespace Instrumentation.DomainDA.DbFramework
{
    public static class DataReaderExtensionMethods
    {
        /// <summary>
        /// Returns the null or value.
        /// </summary>
        /// <param name="boxedString">The boxed string.</param>
        /// <returns></returns>
        public static string ReturnNullOrValue(this object boxedString)
        {
            return Convert.IsDBNull(boxedString) ? null : (string)boxedString;
        }

        /// <summary>
        /// Returns the value if not db null or returns a default value for <typeparamref name="T"/>.
        /// </summary>
        /// <param name="boxedString">The boxed string.</param>
        /// <returns>
        /// default(<typeparamref name="T"/>) or actual value
        /// </returns>
        public static T ReturnDefaultOrValue<T>(this object boxedString)
        {
            if (Convert.IsDBNull(boxedString) || boxedString == null)
                return default(T);

            var type = typeof(T);
            try
            {
                //Check if the boxed object is of the specified type
                if (boxedString.GetType() == type)
                    return (T)boxedString;

                //Check if it is Nullable type as Nullable types aren't supported by change type
                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    return (T)Convert.ChangeType(boxedString, type.GetGenericArguments()[0]);
                }
                //Try to change type
                return (T)Convert.ChangeType(boxedString, type);
            }
            catch (InvalidCastException e)
            {
                //If debug is enabled, log the exception
                //if (RtConfiguration.EnableDebug)
                //{
                //    var logger = new Logger.Logger();
                //    logger.Log(e.Message, category:"Utility Conversion", featureName:"Unknown");
                //}
                return default(T);
            }
        }
    }
}