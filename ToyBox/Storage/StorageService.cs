using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.IO.IsolatedStorage;

namespace ToyBox
{
    public class StorageService : IStorageService
    {
        #region Construction
        public StorageService(IServiceProvider services)
        {
        }
 
        #endregion
        
        #region IStorageService Members
        public string LoadString(string contentName)
        {
            IsolatedStorageFile storageFile;
            string content = null;

#if WINDOWS_PHONE
            storageFile = IsolatedStorageFile.GetUserStoreForApplication();
#else
            storageFile = IsolatedStorageFile.GetUserStoreForDomain();
#endif
            try
            {
                if (!storageFile.FileExists(contentName))
                    return null;

                using (IsolatedStorageFileStream fs = storageFile.OpenFile(contentName, System.IO.FileMode.Open))
                {
                    if (fs != null)
                    {
                        int length = checked((int)fs.Length);
                        byte[] bytes = new byte[length];
                        int count = fs.Read(bytes, 0, length);
                        
                        if (count > 0)
                        {
                            content = Encoding.Unicode.GetString(bytes, 0, bytes.Length);
                        }
                    }
                }
            }
            finally
            {
                storageFile.Dispose();
            }

            return content;
        }

        public void SaveString(string contentName, string content)
        {
            IsolatedStorageFile storageFile;

#if WINDOWS_PHONE
            storageFile = IsolatedStorageFile.GetUserStoreForApplication();
#else
            storageFile = IsolatedStorageFile.GetUserStoreForDomain();
#endif
            try
            {
                // open isolated storage, and write the savefile.
                using (IsolatedStorageFileStream fs = storageFile.CreateFile(contentName))
                {
                    if (fs != null)
                    {
                        byte[] bytes = Encoding.Unicode.GetBytes(content);
                        fs.Write(bytes, 0, bytes.Length);
                    }
                }
            }
            finally
            {
                storageFile.Dispose();
            }
        }

        public void Delete(string contentName)
        {
            IsolatedStorageFile storageFile;

#if WINDOWS_PHONE
            storageFile = IsolatedStorageFile.GetUserStoreForApplication();
#else
            storageFile = IsolatedStorageFile.GetUserStoreForDomain();
#endif
            try
            {
                if (!storageFile.FileExists(contentName))
                    return;

                storageFile.DeleteFile(contentName);
            }
            finally
            {
                storageFile.Dispose();
            }
        }

        #endregion
    }
}
