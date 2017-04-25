﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GameLoader.IO
{
    /// <summary>
    /// PInvoke wrapper for CopyEx
    /// http://msdn.microsoft.com/en-us/library/windows/desktop/aa363852.aspx
    /// </summary>
    public class XCopy
    {
        public class ProgressChangedEventArgs
        {
            public long Progress { get; set; }
            public long Total { get; set; }

            public ProgressChangedEventArgs(long progress, long total)
            {
                Progress = progress;
                Total = total;
            }
        }

        public static void Copy(string source, string destination, bool overwrite, bool nobuffering)
        {
            new XCopy().CopyInternal(source, destination, overwrite, nobuffering, null);
        }

        public static void Copy(string source, string destination, bool overwrite, bool nobuffering, EventHandler<ProgressChangedEventArgs> handler)
        {
            new XCopy().CopyInternal(source, destination, overwrite, nobuffering, handler);
        }

        private event EventHandler Completed;
        private event EventHandler<ProgressChangedEventArgs> ProgressChanged;

        private int IsCancelled;
        private long Progress;
        private string Source;
        private string Destination;

        private XCopy()
        {
            IsCancelled = 0;
        }

        private void CopyInternal(string source, string destination, bool overwrite, bool nobuffering, EventHandler<ProgressChangedEventArgs> handler)
        {
            try
            {
                CopyFileFlags copyFileFlags = CopyFileFlags.COPY_FILE_RESTARTABLE;
                if (!overwrite)
                    copyFileFlags |= CopyFileFlags.COPY_FILE_FAIL_IF_EXISTS;

                if (nobuffering)
                    copyFileFlags |= CopyFileFlags.COPY_FILE_NO_BUFFERING;

                Source = source;
                Destination = destination;

                if (handler != null)
                    ProgressChanged += handler;

                bool result = CopyFileEx(Source, Destination, CopyProgressHandler, IntPtr.Zero, ref IsCancelled, copyFileFlags);
                if (!result)
                    throw new Win32Exception(Marshal.GetLastWin32Error());
            }
            catch (Exception)
            {
                if (handler != null)
                    ProgressChanged -= handler;

                throw;
            }
        }

        private void OnProgressChanged(long progress, long total)
        {
            // only raise an event when progress has changed
            if (progress > Progress)
            {
                Progress = progress;

                var handler = ProgressChanged;
                handler?.Invoke(this, new ProgressChangedEventArgs(progress, total));
            }
        }

        private void OnCompleted()
        {
            var handler = Completed;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        #region PInvoke

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CopyFileEx(string lpExistingFileName, string lpNewFileName, CopyProgressRoutine lpProgressRoutine, IntPtr lpData, ref Int32 pbCancel, CopyFileFlags dwCopyFlags);

        private delegate CopyProgressResult CopyProgressRoutine(long TotalFileSize, long TotalBytesTransferred, long StreamSize, long StreamBytesTransferred, uint dwStreamNumber, CopyProgressCallbackReason dwCallbackReason,
            IntPtr hSourceFile, IntPtr hDestinationFile, IntPtr lpData);

        private enum CopyProgressResult : uint
        {
            PROGRESS_CONTINUE = 0,
            PROGRESS_CANCEL = 1,
            PROGRESS_STOP = 2,
            PROGRESS_QUIET = 3
        }

        private enum CopyProgressCallbackReason : uint
        {
            CALLBACK_CHUNK_FINISHED = 0x00000000,
            CALLBACK_STREAM_SWITCH = 0x00000001
        }

        [Flags]
        private enum CopyFileFlags : uint
        {
            COPY_FILE_FAIL_IF_EXISTS = 0x00000001,
            COPY_FILE_NO_BUFFERING = 0x00001000,
            COPY_FILE_RESTARTABLE = 0x00000002,
            COPY_FILE_OPEN_SOURCE_FOR_WRITE = 0x00000004,
            COPY_FILE_ALLOW_DECRYPTED_DESTINATION = 0x00000008
        }

        private CopyProgressResult CopyProgressHandler(long total, long transferred, long streamSize, long streamByteTrans, uint dwStreamNumber,
            CopyProgressCallbackReason reason, IntPtr hSourceFile, IntPtr hDestinationFile, IntPtr lpData)
        {
            if (reason == CopyProgressCallbackReason.CALLBACK_CHUNK_FINISHED)
                OnProgressChanged(transferred, total);

            if (transferred >= total)
                OnCompleted();

            return CopyProgressResult.PROGRESS_CONTINUE;
        }

        #endregion

    }
}
