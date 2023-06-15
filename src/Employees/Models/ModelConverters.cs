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

    public static EmployeeExtendedJson ToEmployeeExtendedJson(EmployeeEntity employee, EmployeeInformation? employeeInformation, EmergencyContact? emergencyContact)
    {
        return new EmployeeExtendedJson
        {
            Name = employee.Name,
            Email = employee.Email,
            Telephone = employee.Telephone,
            ImageUrl = employee.ImageUrl,
            OfficeName = employee.OfficeName,
            Information = employeeInformation,
            EmergencyContact = emergencyContact
        };
    }

    public static EmployeeInformation ToEmployeeInformation(EmployeeInformationEntity employeeInformation)
    {
        return new EmployeeInformation
        {
            Phone = employeeInformation.Phone,
            AccountNr = employeeInformation.AccountNr,
            Adress = employeeInformation.Adress,
            ZipCode = employeeInformation.ZipCode,
            City = employeeInformation.City,
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