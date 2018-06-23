using System;

namespace FlaxEditor.Workspace.Items
{
    public class SolutionConfigurationAndPlatform : ISolutionConfigurationAndPlatform,
                                                    IEquatable<SolutionConfigurationAndPlatform>
    {
        public readonly string Configuration;

        public readonly string Platform;

        public SolutionConfigurationAndPlatform(string configuration, string platform)
        {
            Configuration = configuration;
            Platform = platform;
        }

        public bool Equals(SolutionConfigurationAndPlatform other)
        {
            if (other == null)
            {
                return false;
            }

            if (this == other)
            {
                return true;
            }

            if (string.Equals(Configuration, other.Configuration))
            {
                return string.Equals(Platform, other.Platform);
            }

            return false;
        }

        public string Uid => $"{Platform}:{Configuration}";

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (this == obj)
            {
                return true;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((SolutionConfigurationAndPlatform)obj);
        }

        public override int GetHashCode()
        {
            return (Configuration.GetHashCode() * 397) ^ Platform.GetHashCode();
        }

        public override string ToString()
        {
            return $"Configuration: {Configuration}, Platform: {Platform}";
        }
    }
}
