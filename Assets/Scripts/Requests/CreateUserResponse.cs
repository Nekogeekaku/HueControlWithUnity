using SimpleJSON;

public class CreateUserResponse 
{

    //Standard responses from Hue API
    //response: [{"error":{"type":101,"address":"/","description":"link button not pressed"}}]
    //response: [{"success":{"username":"<A key>"}}]

    public enum TypeOfError
    {
        known,
        unknown
    }

    public bool IsSuccess { get; private set; }
    public TypeOfError Error { get; private set; }
    public string Unformatted { get; private set; }
    public string exception { get; private set; }



    public string APIKey { get; private set; }
    public int ErrorType { get; private set; }
    public string ErrorAddress { get; private set; }
    public string ErrorDescription { get; private set; }

    public static bool CreateFromJSON(string jsonString, out CreateUserResponse response)
    {

        response = new CreateUserResponse();

        try
        {
            JSONNode root = JSON.Parse(jsonString);
            response.Unformatted = jsonString;
            if (root.IsArray) //Philips Hue always return an array on this particular API with only one element
            {

                JSONNode mainElement = root[0];

                if (mainElement.HasKey("success"))
                {
                    response.IsSuccess = true;
                    response.APIKey = mainElement["success"]["username"];


                }
                else if (mainElement.HasKey("error"))
                {
                    response.IsSuccess = false;
                    response.Error = TypeOfError.known;
                    response.ErrorType = mainElement["error"]["type"].AsInt;
                    response.ErrorAddress = mainElement["error"]["adress"];
                    response.ErrorDescription = mainElement["error"]["description"];
                }
                else
                { //undefined error
                    response.IsSuccess = false;
                    response.Error = TypeOfError.unknown;
                }


            }
            else
            {
                return false;
            }
        }
        catch (System.Exception ex)
        {

            response.IsSuccess = false;
            response.Error = TypeOfError.unknown;
            response.Unformatted = jsonString;
            response.exception = ex.ToString();
            return false;
        }

        return true;
    }
}
