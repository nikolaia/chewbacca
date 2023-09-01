// ReSharper disable InconsistentNaming

using ApplicationCore.Models;

#pragma warning disable CS8618
namespace Infrastructure.ApiClients.DTOs;

public class CVPartnerUserDTO
{
    public string user_id { get; set; }
    public string _id { get; set; }
    public string id { get; set; }
    public string company_id { get; set; }
    public string company_name { get; set; }
    public List<string> company_subdomains { get; set; }
    public List<object> company_group_ids { get; set; }
    public string email { get; set; }
    public object external_unique_id { get; set; }
    public object upn { get; set; }
    public bool deactivated { get; set; }
    public bool deactivated_at { get; set; }
    public DateTime created_at { get; set; }
    public DateTime updated_at { get; set; }
    public string role { get; set; }
    public List<string> roles { get; set; }
    public List<object> role_allowed_office_ids { get; set; }
    public List<object> role_allowed_tag_ids { get; set; }
    public string office_id { get; set; }
    public string office_name { get; set; }
    public string country_id { get; set; }
    public string country_code { get; set; }
    public string language_code { get; set; }
    public List<string> language_codes { get; set; }
    public string international_toggle { get; set; }
    public string preferred_download_format { get; set; }
    public List<string> masterdata_languages { get; set; }
    public bool expand_proposals_toggle { get; set; }
    public List<string> selected_office_ids { get; set; }
    public bool include_officeless_reference_projects { get; set; }
    public List<object> selected_tag_ids { get; set; }
    public object override_language_code { get; set; }
    public string default_cv_template_id { get; set; }
    public Image image { get; set; }
    public string name { get; set; }
    public string telephone { get; set; }
    public string default_cv_id { get; set; }
}

public class CVPartnerCvDTO
{
    public string _id { get; set; }
    public List<Blog> blogs { get; set; }
    public int born_day { get; set; }
    public int born_month { get; set; }
    public int? born_year { get; set; }
    public string bruker_id { get; set; }
    public List<Certification> certifications { get; set; }
    public DateTime created_at { get; set; }
    public List<object> custom_tag_ids { get; set; }
    public List<CvRole> cv_roles { get; set; }
    public bool @default { get; set; }
    public List<Education> educations { get; set; }
    public object imported_date { get; set; }
    public List<KeyQualification> key_qualifications { get; set; }
    public object landline { get; set; }
    public List<Language> languages { get; set; }
    public object level { get; set; }
    public object locked_at { get; set; }
    public object locked_until { get; set; }
    public object modifier_id { get; set; }
    public NameMultilang name_multilang { get; set; }
    public Nationality nationality { get; set; }
    public string navn { get; set; }
    public object order { get; set; }
    public DateTime? owner_updated_at { get; set; }
    public DateTime? owner_updated_at_significant { get; set; }
    public PlaceOfResidence place_of_residence { get; set; }
    public List<Position> positions { get; set; }
    public List<CvPartnerPresentationDTO>? presentations { get; set; }
    public List<ProjectExperience>? project_experiences { get; set; }
    public List<Recommendation> recommendations { get; set; }
    public List<Technology> technologies { get; set; }
    public string telefon { get; set; }
    public object tilbud_id { get; set; }
    public Title title { get; set; }
    public string twitter { get; set; }
    public DateTime updated_at { get; set; }
    public int version { get; set; }
    public List<WorkExperience>? work_experiences { get; set; }
    public string name { get; set; }
    public string user_id { get; set; }
    public string company_id { get; set; }
    public object external_unique_id { get; set; }
    public string email { get; set; }
    public string country_code { get; set; }
    public string language_code { get; set; }
    public List<string> language_codes { get; set; }
    public object proposal { get; set; }
    public List<object> custom_tags { get; set; }
    public string updated_ago { get; set; }
    public string template_document_type { get; set; }
    public string default_word_template_id { get; set; }
    public object default_ppt_template_id { get; set; }
    public List<object> courses { get; set; }
    public List<object> highlighted_roles { get; set; }
    public Image image { get; set; }
    public bool can_write { get; set; }
}

public class CvPartnerPresentationDTO
{
    public string _id { get; set; }
    public DateTime created_at { get; set; }
    public Description description { get; set; }
    public bool disabled { get; set; }
    public bool diverged_from_master { get; set; }
    public object external_unique_id { get; set; }
    public LongDescription long_description { get; set; }
    public object modifier_id { get; set; }
    public string month { get; set; }
    public int order { get; set; }
    public object origin_id { get; set; }
    public DateTime? owner_updated_at { get; set; }
    public bool recently_added { get; set; }
    public bool starred { get; set; }
    public DateTime updated_at { get; set; }
    public int version { get; set; }
    public string year { get; set; }
}

public class FitThumb
{
    public string url { get; set; }
}

public class Image
{
    public string? url { get; set; }
    public Thumb thumb { get; set; }
    public FitThumb fit_thumb { get; set; }
    public Large large { get; set; }
    public SmallThumb small_thumb { get; set; }
}

public class Large
{
    public string url { get; set; }
}

public class SmallThumb
{
    public string url { get; set; }
}

public class Thumb
{
    public string url { get; set; }
}

public class Blog
{
    public string _id { get; set; }
    public DateTime created_at { get; set; }
    public bool disabled { get; set; }
    public bool diverged_from_master { get; set; }
    public object external_unique_id { get; set; }
    public LongDescription long_description { get; set; }
    public object modifier_id { get; set; }
    public object month { get; set; }
    public Name name { get; set; }
    public int order { get; set; }
    public object origin_id { get; set; }
    public DateTime? owner_updated_at { get; set; }
    public bool recently_added { get; set; }
    public bool starred { get; set; }
    public DateTime updated_at { get; set; }
    public string url { get; set; }
    public int version { get; set; }
    public object year { get; set; }
}

public class Category
{
    public string no { get; set; }
}

public class Certification
{
    public string _id { get; set; }
    public DateTime created_at { get; set; }
    public bool disabled { get; set; }
    public bool diverged_from_master { get; set; }
    public object external_unique_id { get; set; }
    public LongDescription long_description { get; set; }
    public object modifier_id { get; set; }
    public string month { get; set; }
    public string month_expire { get; set; }
    public Name name { get; set; }
    public int order { get; set; }
    public Organiser organiser { get; set; }
    public object origin_id { get; set; }
    public DateTime? owner_updated_at { get; set; }
    public bool recently_added { get; set; }
    public bool starred { get; set; }
    public DateTime updated_at { get; set; }
    public int version { get; set; }
    public string year { get; set; }
    public string year_expire { get; set; }
    public List<object> attachments { get; set; }
    public bool is_expired { get; set; }
}

public class Customer
{
    public string no { get; set; }
    public string @int { get; set; }
}

public class CustomerAnonymized
{
}

public class CustomerDescription
{
}

public class CustomerValueProposition
{
    public string no { get; set; }
    public string @int { get; set; }
}

public class CvRole
{
    public string _id { get; set; }
    public DateTime created_at { get; set; }
    public bool disabled { get; set; }
    public bool diverged_from_master { get; set; }
    public object modifier_id { get; set; }
    public Name name { get; set; }
    public int order { get; set; }
    public object origin_id { get; set; }
    public DateTime? owner_updated_at { get; set; }
    public bool recently_added { get; set; }
    public bool starred { get; set; }
    public object starred_order { get; set; }
    public DateTime updated_at { get; set; }
    public int? version { get; set; }
    public int years_of_experience { get; set; }
    public int years_of_experience_offset { get; set; }
    public List<ProjectExperience> project_experiences { get; set; }
}

public class Degree
{
    public string no { get; set; }
}

public class Description
{
    public string? no { get; set; }
    public string @int { get; set; }
}

public class Education
{
    public string _id { get; set; }
    public DateTime created_at { get; set; }
    public Degree degree { get; set; }
    public Description description { get; set; }
    public bool disabled { get; set; }
    public bool diverged_from_master { get; set; }
    public object external_unique_id { get; set; }
    public object modifier_id { get; set; }
    public string month_from { get; set; }
    public string month_to { get; set; }
    public int order { get; set; }
    public object origin_id { get; set; }
    public object owner_updated_at { get; set; }
    public bool recently_added { get; set; }
    public School school { get; set; }
    public bool starred { get; set; }
    public DateTime updated_at { get; set; }
    public int version { get; set; }
    public string year_from { get; set; }
    public string year_to { get; set; }
    public List<object> attachments { get; set; }
}

public class Employer
{
    public string no { get; set; }
    public string @int { get; set; }
}

public class Industry
{
    public string no { get; set; }
}

public class KeyQualification
{
    public string _id { get; set; }
    public DateTime created_at { get; set; }
    public bool disabled { get; set; }
    public bool diverged_from_master { get; set; }
    public object external_unique_id { get; set; }
    public Label label { get; set; }
    public LongDescription long_description { get; set; }
    public object modifier_id { get; set; }
    public int order { get; set; }
    public object origin_id { get; set; }
    public object owner_updated_at { get; set; }
    public bool recently_added { get; set; }
    public bool starred { get; set; }
    public TagLine tag_line { get; set; }
    public DateTime updated_at { get; set; }
    public int version { get; set; }
}

public class Label
{
    public string no { get; set; }
    public string @int { get; set; }
}

public class Language
{
    public string _id { get; set; }
    public DateTime created_at { get; set; }
    public bool disabled { get; set; }
    public bool diverged_from_master { get; set; }
    public object external_unique_id { get; set; }
    public Level level { get; set; }
    public object modifier_id { get; set; }
    public Name name { get; set; }
    public int order { get; set; }
    public object origin_id { get; set; }
    public object owner_updated_at { get; set; }
    public bool recently_added { get; set; }
    public bool starred { get; set; }
    public DateTime updated_at { get; set; }
    public int version { get; set; }
}

public class Level
{
    public string no { get; set; }
}

public class LongDescription
{
    public string? no { get; set; }
    public string @int { get; set; }
}

public class Name
{
    public string? no { get; set; }
    public string @int { get; set; }
}

public class NameMultilang
{
}

public class Nationality
{
    public string no { get; set; }
}

public class Organiser
{
    public string no { get; set; }
}

public class PlaceOfResidence
{
    public string no { get; set; }
}

public class Position
{
    public string _id { get; set; }
    public DateTime created_at { get; set; }
    public Description description { get; set; }
    public bool disabled { get; set; }
    public bool diverged_from_master { get; set; }
    public object external_unique_id { get; set; }
    public object modifier_id { get; set; }
    public Name name { get; set; }
    public int order { get; set; }
    public object origin_id { get; set; }
    public DateTime? owner_updated_at { get; set; }
    public bool recently_added { get; set; }
    public bool starred { get; set; }
    public DateTime updated_at { get; set; }
    public int version { get; set; }
    public string year_from { get; set; }
    public string year_to { get; set; }
}

public class ProjectExperience
{
    public string _id { get; set; }
    public List<Role>? roles { get; set; }
    public bool diverged_from_master { get; set; }
    public object area_amt { get; set; }
    public object area_unit { get; set; }
    public DateTime created_at { get; set; }
    public Customer customer { get; set; }
    public CustomerAnonymized customer_anonymized { get; set; }
    public CustomerDescription customer_description { get; set; }
    public string customer_selected { get; set; }
    public CustomerValueProposition customer_value_proposition { get; set; }
    public Description description { get; set; }
    public bool disabled { get; set; }
    public List<object> exclude_tags { get; set; }
    public object expected_roll_off_date { get; set; }
    public string extent_hours { get; set; }
    public object external_unique_id { get; set; }
    public Industry industry { get; set; }
    public object location_country_code { get; set; }
    public LongDescription long_description { get; set; }
    public object modifier_id { get; set; }
    public string month_from { get; set; }
    public string month_to { get; set; }
    public int order { get; set; }
    public object origin_id { get; set; }
    public DateTime? owner_updated_at { get; set; }
    public string percent_allocated { get; set; }
    public List<ProjectExperienceSkill>? project_experience_skills { get; set; }
    public string project_extent_amt { get; set; }
    public string project_extent_currency { get; set; }
    public string project_extent_hours { get; set; }
    public ProjectType project_type { get; set; }
    public bool recently_added { get; set; }
    public object related_work_experience_id { get; set; }
    public bool starred { get; set; }
    public string total_extent_amt { get; set; }
    public string total_extent_currency { get; set; }
    public string total_extent_hours { get; set; }
    public DateTime updated_at { get; set; }
    public int version { get; set; }
    public string year_from { get; set; }
    public string year_to { get; set; }
    public List<object> images { get; set; }
}

public class ProjectExperienceSkill
{
    public string _id { get; set; }
    public int base_duration_in_years { get; set; }
    public object modifier_id { get; set; }
    public int offset_duration_in_years { get; set; }
    public int order { get; set; }
    public int proficiency { get; set; }
    public Tags tags { get; set; }
    public int total_duration_in_years { get; set; }
    public int? version { get; set; }
}

public class ProjectType
{
}

public class Recommendation
{
    public string _id { get; set; }
    public DateTime created_at { get; set; }
    public bool disabled { get; set; }
    public bool diverged_from_master { get; set; }
    public object external_unique_id { get; set; }
    public LongDescription long_description { get; set; }
    public object modifier_id { get; set; }
    public int order { get; set; }
    public object origin_id { get; set; }
    public object owner_updated_at { get; set; }
    public bool recently_added { get; set; }
    public Recommender recommender { get; set; }
    public bool starred { get; set; }
    public DateTime updated_at { get; set; }
    public int version { get; set; }
}

public class Recommender
{
    public string no { get; set; }
}

public class Role
{
    public string _id { get; set; }
    public DateTime? created_at { get; set; }
    public string cv_role_id { get; set; }
    public bool disabled { get; set; }
    public bool diverged_from_master { get; set; }
    public LongDescription long_description { get; set; }
    public object modifier_id { get; set; }
    public Name? name { get; set; }
    public int? order { get; set; }
    public object origin_id { get; set; }
    public bool recently_added { get; set; }
    public bool starred { get; set; }
    public Summary summary { get; set; }
    public DateTime? updated_at { get; set; }
    public int? version { get; set; }
}

public class School
{
    public string no { get; set; }
}

public class Summary
{
    public string? no { get; set; }
}

public class TagLine
{
}

public class Tags
{
    public string? no { get; set; }
}

public class Technology
{
    public string _id { get; set; }
    public Category category { get; set; }
    public DateTime? created_at { get; set; }
    public bool disabled { get; set; }
    public bool diverged_from_master { get; set; }
    public List<object> exclude_tags { get; set; }
    public object external_unique_id { get; set; }
    public object modifier_id { get; set; }
    public int? order { get; set; }
    public object origin_id { get; set; }
    public DateTime? owner_updated_at { get; set; }
    public bool recently_added { get; set; }
    public bool starred { get; set; }
    public List<TechnologySkill> technology_skills { get; set; }
    public bool uncategorized { get; set; }
    public DateTime updated_at { get; set; }
    public int version { get; set; }
}

public class TechnologySkill
{
    public string _id { get; set; }
    public int base_duration_in_years { get; set; }
    public object modifier_id { get; set; }
    public int offset_duration_in_years { get; set; }
    public int order { get; set; }
    public int proficiency { get; set; }
    public Tags tags { get; set; }
    public int total_duration_in_years { get; set; }
    public int? version { get; set; }
}

public class Title
{
    public string no { get; set; }
}

public class WorkExperience
{
    public string _id { get; set; }
    public DateTime created_at { get; set; }
    public Description description { get; set; }
    public bool disabled { get; set; }
    public bool diverged_from_master { get; set; }
    public Employer employer { get; set; }
    public object external_unique_id { get; set; }
    public LongDescription long_description { get; set; }
    public object modifier_id { get; set; }
    public string month_from { get; set; }
    public string month_to { get; set; }
    public int order { get; set; }
    public object origin_id { get; set; }
    public DateTime? owner_updated_at { get; set; }
    public bool recently_added { get; set; }
    public bool starred { get; set; }
    public DateTime updated_at { get; set; }
    public int version { get; set; }
    public string year_from { get; set; }
    public string year_to { get; set; }
}

public static class CvDtoConverter
{
    private static List<Presentation> ToPresentations(CVPartnerCvDTO cv)
    {
        if (cv.presentations == null)
        {
            return new List<Presentation>();
        }

        return cv.presentations.Select(dto => new Presentation
        {
            Description = dto.long_description.no ?? "",
            Year = dto.year,
            Month = dto.month,
            Title = dto.description.no ?? "",
            Id = dto._id
        }).ToList();
    }

    private static List<ApplicationCore.Models.WorkExperience> ToWorkExperience(CVPartnerCvDTO cv)
    {
        if (cv.work_experiences == null)
        {
            return new List<ApplicationCore.Models.WorkExperience>();
        }

        return cv.work_experiences.Select(dto => new ApplicationCore.Models.WorkExperience()
        {
            Description = dto.long_description.no ?? "",
            MonthFrom = dto.month_from,
            YearFrom = dto.year_from,
            MonthTo = dto.month_to,
            YearTo = dto.year_to,
            Title = dto.description.no ?? "",
            Id = dto._id
        }).ToList();
    }


    private static List<ApplicationCore.Models.ProjectExperience> CreateProjectExperienceFromCv(CVPartnerCvDTO cv)
    {
        if (cv.project_experiences == null)
        {
            return new List<ApplicationCore.Models.ProjectExperience>();
        }

        return cv.project_experiences.Select(dto => new ApplicationCore.Models.ProjectExperience
        {
            Description = dto.long_description.no ?? "",
            MonthFrom = dto.month_from,
            YearFrom = dto.year_from,
            MonthTo = dto.month_to,
            YearTo = dto.year_to,
            Title = dto.description.no ?? "",
            Roles = CreateProjectExperienceRolesFromProject(dto),
            Competencies = CreateCompetenciesFromProject(dto),
            Id = dto._id
        }).ToList();
    }

    private static HashSet<string> CreateCompetenciesFromProject(ProjectExperience dto)
    {
        if (dto.project_experience_skills == null)
        {
            return new HashSet<string>();
        }


        return dto.project_experience_skills
            .Select(skill => skill.tags.no)
            .Where(tag => tag is not null)
            .Select(tag => tag!.ToUpper()) // Ignore case because tags are unique
            .ToHashSet();
    }

    private static List<ProjectExperienceRole> CreateProjectExperienceRolesFromProject(
        ProjectExperience dto)
    {
        if (dto.roles == null)
        {
            return new List<ProjectExperienceRole>();
        }

        return dto.roles.Select(role => new ProjectExperienceRole()
        {
            Description = role.long_description.no ?? "", Title = role.name?.no ?? "", Id = role._id
        }).ToList();
    }

    private static List<ApplicationCore.Models.Certification> createCertificationFromCv(CVPartnerCvDTO dto)
    {
        if (dto.certifications == null)
        {
            return new List<ApplicationCore.Models.Certification>();
        }

        return dto.certifications.Select(cert => new ApplicationCore.Models.Certification
        {
            Id = cert._id,
            Description = cert.long_description.no ?? "",
            Title = cert.name?.no ?? "",
            IssuedMonth = cert.month,
            IssuedYear = cert.year,
            ExpiryDate = DateFromNullableStrings(cert.month_expire, cert.year_expire)
        }).ToList();
    }

    private static DateTime? DateFromNullableStrings(string? month, string? year)
    {
        if (string.IsNullOrEmpty(year) || !int.TryParse(year, out int yearValue))
        {
            return null;
        }

        if (string.IsNullOrEmpty(month) || !int.TryParse(month, out int monthValue))
        {
            return new DateTime(yearValue, 1, 1);
        }

        return new DateTime(yearValue, monthValue, 1);
    }


    public static Cv ToCv(CVPartnerCvDTO cvPartnerCv)
    {
        return new Cv
        {
            Email = cvPartnerCv.email,
            Presentations = ToPresentations(cvPartnerCv),
            WorkExperiences = ToWorkExperience(cvPartnerCv),
            ProjectExperiences = CreateProjectExperienceFromCv(cvPartnerCv),
            Certifiactions = createCertificationFromCv(cvPartnerCv)
        };
    }
}