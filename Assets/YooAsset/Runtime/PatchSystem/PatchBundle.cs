﻿using System;
using System.Linq;

namespace YooAsset
{
	[Serializable]
	internal class PatchBundle
	{
		/// <summary>
		/// 资源包名称
		/// </summary>
		public string BundleName;

		/// <summary>
		/// 文件哈希值
		/// </summary>
		public string FileHash;

		/// <summary>
		/// 文件校验码
		/// </summary>
		public string FileCRC;

		/// <summary>
		/// 文件大小（字节数）
		/// </summary>
		public long FileSize;

		/// <summary>
		/// 是否为原生文件
		/// </summary>
		public bool IsRawFile;

		/// <summary>
		/// 加载方法
		/// </summary>
		public byte LoadMethod;

		/// <summary>
		/// 资源包的分类标签
		/// </summary>
		public string[] Tags;

		/// <summary>
		/// 文件名称
		/// </summary>	
		public string FileName { private set; get; }

		/// <summary>
		/// 缓存文件路径
		/// </summary>
		private string _cachedFilePath;
		public string CachedFilePath
		{
			get
			{
				if (string.IsNullOrEmpty(_cachedFilePath) == false)
					return _cachedFilePath;

				string cacheRoot = PersistentHelper.GetCacheFolderPath();
				_cachedFilePath = $"{cacheRoot}/{FileName}";
				return _cachedFilePath;
			}
		}

		/// <summary>
		/// 内置文件路径
		/// </summary>
		private string _streamingFilePath;
		public string StreamingFilePath
		{
			get
			{
				if (string.IsNullOrEmpty(_streamingFilePath) == false)
					return _streamingFilePath;

				_streamingFilePath = PathHelper.MakeStreamingLoadPath(FileName);
				return _streamingFilePath;
			}
		}


		public PatchBundle(string bundleName, string fileHash, string fileCRC, long fileSize, bool isRawFile, byte loadMethod, string[] tags)
		{
			BundleName = bundleName;
			FileHash = fileHash;
			FileCRC = fileCRC;
			FileSize = fileSize;
			IsRawFile = isRawFile;
			LoadMethod = loadMethod;
			Tags = tags;
		}

		/// <summary>
		/// 解析文件名称
		/// </summary>
		public void ParseFileName(int nameStype)
		{
			FileName = PatchManifest.CreateBundleFileName(nameStype, BundleName, FileHash);
		}

		/// <summary>
		/// 是否包含Tag
		/// </summary>
		public bool HasTag(string[] tags)
		{
			if (tags == null || tags.Length == 0)
				return false;
			if (Tags == null || Tags.Length == 0)
				return false;

			foreach (var tag in tags)
			{
				if (Tags.Contains(tag))
					return true;
			}
			return false;
		}

		/// <summary>
		/// 是否包含任意Tags
		/// </summary>
		public bool HasAnyTags()
		{
			if (Tags != null && Tags.Length > 0)
				return true;
			else
				return false;
		}

		/// <summary>
		/// 检测资源包文件内容是否相同
		/// </summary>
		public bool Equals(PatchBundle otherBundle)
		{
			if (FileHash == otherBundle.FileHash)
				return true;

			return false;
		}
	}
}