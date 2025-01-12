using QueryableCore.DTOs;
using QueryableCore.RepositoriesInterfaces;
using QueryableCore.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;
using System.Reflection;
using QueryableCore.Enums;

namespace QueryableCore.Services
{
    public class BuildingsService : IBuildingsService
    {
        private readonly IBuildingsRepository _buildingsRepository;

        public BuildingsService(IBuildingsRepository buildingsRepository)
        {
            _buildingsRepository = buildingsRepository;
        }
        public List<BuildingDto> GetBuildings(BuildingsRequestData requestData)
        {
            return _buildingsRepository.GetFilteredAndSortedBuildings(requestData);
        }
        public int? CreateBuilding(BuildingDto buildingDto)
        {
            return _buildingsRepository.CreateBuilding(buildingDto);
        }

        public List<string> GetRequiredFields(Type modelType)
        {
            return GetRequiredFieldsRecursive(modelType, string.Empty);
        }

        private List<string> GetRequiredFieldsRecursive(Type modelType, string parentName)
        {
            var requiredFields = new List<string>();

            var properties = modelType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                var isRequired = Attribute.IsDefined(property, typeof(System.ComponentModel.DataAnnotations.RequiredAttribute));
                var propertyName = string.IsNullOrWhiteSpace(parentName) ? property.Name : $"{parentName}.{property.Name}";

                if (isRequired)
                {
                    requiredFields.Add(propertyName);
                }

                if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                {
                    requiredFields.AddRange(GetRequiredFieldsRecursive(property.PropertyType, propertyName));
                }
            }

            return requiredFields;
        }
        public List<string> GetClassMembers(string modelName, bool? isRequired, AccessModifier[] accessModifiers, MemberType[] memberTypes, bool? isStatic)
        {
            var modelType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.Name.Equals(modelName, StringComparison.OrdinalIgnoreCase));

            if (modelType == null)
                throw new ArgumentException($"Model type '{modelName}' not found.");

            return GetClassMembersInternal(modelType, isRequired, accessModifiers, memberTypes, isStatic);
        }

        private List<string> GetClassMembersInternal(Type modelType, bool? isRequired, AccessModifier[] accessModifiers, MemberType[] memberTypes, bool? isStatic)
        {
            var members = new List<string>();

            var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
            var allMembers = modelType.GetMembers(bindingFlags);

            foreach (var member in allMembers)
            {
                var memberInfo = string.Empty;
                var attributes = member.GetCustomAttributes();

                if (memberTypes != null && memberTypes.Length > 0 &&
                    !memberTypes.Any(mt => MatchesMemberType(member, mt)))
                {
                    continue;
                }

                //фільтрація за модиф. доступу
                var modifiers = GetAccessModifiersEnum(member);
                if (accessModifiers != null && accessModifiers.Length > 0 &&
                    !accessModifiers.Any(am => modifiers.Contains(am)))
                {
                    continue;
                }

                //фільтрація ща статичністю
                if (isStatic.HasValue)
                {
                    bool isMemberStatic = member is MethodBase method ? method.IsStatic :
                                          member is FieldInfo field ? field.IsStatic :
                                          member is PropertyInfo property && property.GetGetMethod(true)?.IsStatic == true;

                    if (isStatic.Value != isMemberStatic)
                    {
                        continue;
                    }
                }

                //фільтрація за атрибутом required
                if (isRequired.HasValue)
                {
                    var hasRequired = attributes.Any(attr => attr is System.ComponentModel.DataAnnotations.RequiredAttribute);
                    if (isRequired.Value && !hasRequired)
                    {
                        continue;
                    }
                    if (!isRequired.Value && hasRequired)
                    {
                        continue;
                    }
                }

                        
                memberInfo = $"{member.MemberType} {string.Join(" ", modifiers)} {member.Name}";
                members.Add(memberInfo);
            }

            return members;
        }

        private bool MatchesMemberType(MemberInfo member, MemberType memberType)
        {
            return memberType switch
            {
                MemberType.Field => member is FieldInfo,
                MemberType.Property => member is PropertyInfo,
                MemberType.Method => member is MethodInfo,
                MemberType.Event => member is EventInfo,
                MemberType.Constructor => member is ConstructorInfo,
                _ => false
            };
        }

        private List<AccessModifier> GetAccessModifiersEnum(MemberInfo member)
        {
            var modifiers = new List<AccessModifier>();

            if (member is MethodBase method)
            {
                if (method.IsPublic) modifiers.Add(AccessModifier.Public);
                if (method.IsPrivate) modifiers.Add(AccessModifier.Private);
                if (method.IsFamily) modifiers.Add(AccessModifier.Protected);
                if (method.IsAssembly) modifiers.Add(AccessModifier.Internal);
                if (method.IsStatic) modifiers.Add(AccessModifier.Static);
            }
            else if (member is FieldInfo field)
            {
                if (field.IsPublic) modifiers.Add(AccessModifier.Public);
                if (field.IsPrivate) modifiers.Add(AccessModifier.Private);
                if (field.IsFamily) modifiers.Add(AccessModifier.Protected);
                if (field.IsAssembly) modifiers.Add(AccessModifier.Internal);
                if (field.IsStatic) modifiers.Add(AccessModifier.Static);
            }
            else if (member is PropertyInfo property)
            {
                var getMethod = property.GetGetMethod(true);
                if (getMethod != null)
                {
                    if (getMethod.IsPublic) modifiers.Add(AccessModifier.Public);
                    if (getMethod.IsPrivate) modifiers.Add(AccessModifier.Private);
                    if (getMethod.IsFamily) modifiers.Add(AccessModifier.Protected);
                    if (getMethod.IsAssembly) modifiers.Add(AccessModifier.Internal);
                    if (getMethod.IsStatic) modifiers.Add(AccessModifier.Static);
                }
            }

            return modifiers;
        }
    }

    public enum AccessModifier
    {
        Public,
        Private,
        Protected,
        Internal,
        Static
    }

    public enum MemberType
    {
        Field,
        Property,
        Method,
        Event,
        Constructor
    }


}
