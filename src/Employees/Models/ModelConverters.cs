namespace Employees.Models;

public static class ModelConverters
{
    public static Employee ToEmployee(EmployeeEntity employeeEntity)
    {
        return new Employee
        {
            Name = employeeEntity.Name,
            Email = employeeEntity.Email,
            Telephone = employeeEntity.Telephone,
            ImageUrl = employeeEntity.ImageUrl,
            OfficeName = employeeEntity.OfficeName,
            StartDate = employeeEntity.StartDate,
            EndDate = employeeEntity.EndDate
        };
    }

    public static EmployeeJson ToEmployeeJson(Employee employee)
    {
        return new EmployeeJson
        {
            Name = employee.Name,
            Email = employee.Email,
            Telephone = employee.Telephone,
            ImageUrl = employee.ImageUrl,
            OfficeName = employee.OfficeName,
            StartDate = employee.StartDate,
        };
    }

    public static EmployeeJson ToEmployeeJson(EmployeeEntity employee)
    {
        return new EmployeeJson
        {
            Name = employee.Name,
            Email = employee.Email,
            Telephone = employee.Telephone,
            ImageUrl = employee.ImageUrl,
            OfficeName = employee.OfficeName,
            StartDate = employee.StartDate,
        };
    }

    public static EmployeeExtendedJson ToEmployeeExtendedJson(EmployeeEntity employee, EmergencyContact? emergencyContact, AllergiesAndDietaryPreferences allergiesAndDietaryPreferences)
    {
        return new EmployeeExtendedJson
        {
            Name = employee.Name,
            Email = employee.Email,
            Telephone = employee.Telephone,
            ImageUrl = employee.ImageUrl,
            OfficeName = employee.OfficeName,
            AccountNumber = employee.AccountNumber,
            Address = employee.Address,
            ZipCode = employee.ZipCode,
            City = employee.City,
            EmergencyContact = emergencyContact,
            AllergiesAndDietaryPreferences = allergiesAndDietaryPreferences
        };
    }

    public static EmergencyContact ToEmergencyContact(EmergencyContactEntity emergencyContact)
    {
        return new EmergencyContact
        {
            Name = emergencyContact.Name,
            Phone = emergencyContact.Phone,
            Relation = emergencyContact.Relation,
            Comment = emergencyContact.Comment,
        };
    }
}