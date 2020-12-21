using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Redstone.CLI.Registries.Converters
{
    internal class CSharpEnumConverter : IConverter
    {
        /// <summary>
        /// Gets the enum name.
        /// </summary>
        public string EnumName { get; }

        /// <summary>
        /// Gets the registry values.
        /// </summary>
        public IDictionary<string, int> Values { get; }

        /// <summary>
        /// Gets the enum namespace.
        /// </summary>
        /// <remarks>
        /// The namespace is optional.
        /// </remarks>
        public string? EnumNamespace { get; }

        public CSharpEnumConverter(string enumName, IDictionary<string, int> values, string? enumNamespace = null)
        {
            EnumName = enumName;
            Values = values;
            EnumNamespace = enumNamespace;
        }

        public string Convert()
        {
            NamespaceDeclarationSyntax @namespace = null!;

            if (!string.IsNullOrEmpty(EnumNamespace))
            {
                @namespace = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(EnumNamespace));
            }

            EnumMemberDeclarationSyntax[] enumMembers = Values
                .OrderBy(x => x.Value)
                .Select(x => SyntaxFactory.EnumMemberDeclaration(x.Key))
                .ToArray();
            EnumDeclarationSyntax @enum = SyntaxFactory.EnumDeclaration(EnumName)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddMembers(enumMembers);

            var result = @namespace is not null
                ? @namespace.AddMembers(@enum).NormalizeWhitespace().ToFullString()
                : @enum.NormalizeWhitespace().ToFullString();
            
            return result;
        }
    }
}
