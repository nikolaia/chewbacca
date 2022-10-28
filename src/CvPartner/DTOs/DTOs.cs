namespace CvPartner.DTOs;

public class FitThumb
{
    public string url { get; set; }
}

public class Image
{
    public string url { get; set; }
    public Thumb thumb { get; set; }
    public FitThumb fit_thumb { get; set; }
    public Large large { get; set; }
    public SmallThumb small_thumb { get; set; }
}

public class Large
{
    public string url { get; set; }
}

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

public class SmallThumb
{
    public string url { get; set; }
}

public class Thumb
{
    public string url { get; set; }
}