using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CountryTrash.Graphics
{
	public sealed class ResourceManager
	{
		private static readonly Regex pathSpecifier = new Regex(@"^(?<path>\/(?:\w+\/)*)(?<file>\w+)$", RegexOptions.Compiled);

		private readonly Dictionary<string, Resource> resources = new Dictionary<string, Resource>();
		private readonly string root;

		public ResourceManager(string root)
		{
			this.root = root;
			// Ensure the root folder ends with a '/' so we can append any local path without problems
			if (this.root.EndsWith("/") == false)
				this.root += "/";
		}

		private string GetResourceFileName<T>(string resourcePath)
			where T : Resource, ILoadableResource, new()
		{
			var path = pathSpecifier.Match(resourcePath);
			if (path.Success == false)
				throw new ArgumentException("Path is not in a valid format!", "resourcePath");
			var extensions = (ResourceExtensionAttribute[])typeof(T).GetCustomAttributes(typeof(ResourceExtensionAttribute), true);
			foreach (var extension in extensions.SelectMany(e => e.Extensions))
			{
				string fileName = this.root + resourcePath + extension;
				if (File.Exists(fileName) == false)
					continue;
				return fileName;
			}
			return null;
		}

		/// <summary>
		/// Checks if a resource exists.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="resourcePath"></param>
		/// <returns></returns>
		public bool Exist<T>(string resourcePath)
			where T : Resource, ILoadableResource, new()
		{
			if (this.resources.ContainsKey(GetResourceID<T>(resourcePath)))
				return true;
			return (GetResourceFileName<T>(resourcePath) != null);
		}

		/// <summary>
		/// Gets a resource by its resource path. Uses caching.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="resourcePath"></param>
		/// <returns></returns>
		public T Get<T>(string resourcePath)
			where T : Resource, ILoadableResource, new()
		{
			var path = pathSpecifier.Match(resourcePath);
			if (path.Success == false)
				throw new ArgumentException("Path is not in a valid format!", "resourcePath");
			string resourceID = GetResourceID<T>(resourcePath);

			if (this.resources.ContainsKey(resourceID) == false)
			{
				var fileName = this.GetResourceFileName<T>(resourcePath);
				if (fileName == null)
					throw new FileNotFoundException("The given resource does not exist", resourcePath);

				var res = new T();
				using (var fs = File.Open(fileName, FileMode.Open, FileAccess.Read))
				{
					res.Load(this, fs);
				}
				this.resources.Add(resourceID, res);
			}

			var resource = this.resources[resourceID];
			if (typeof(T).IsAssignableFrom(resource.GetType()) == false)
				throw new InvalidOperationException($"The type {resource.GetType().Name} is not assignable to {typeof(T).Name}.");
			return (T)resource;
		}

		private static string GetResourceID<T>(string resourcePath) where T : Resource, ILoadableResource, new()
		{
			return $"{typeof(T).Name}:{resourcePath}";
		}

		/// <summary>
		/// Checks if a resource path is valid.
		/// </summary>
		/// <param name="resourcePath"></param>
		/// <returns></returns>
		public static bool IsValidResourcePath(string resourcePath)
		{
			return pathSpecifier.IsMatch(resourcePath);
		}
	}
}