
namespace WeddingInvitation.Infrastructure.Unity
{
    //Each Status-Code is described below, 
    //including a description of which method(s) 
    //it can follow and any metainformation required in the response. 
    public enum ApiStatusCodes
    {
        //The API must return a 200 FOUND if:
        //The resource has been found (for GET requests)
        //The resource has been deleted (for DELETE requests)
        Found=200,
        //The API must return a 201 CREATED if:
        //The resource has been created (for POST/PUT requests only) and is available on the
        //site immediately
        Created=201,
        //The API must return a 202 ACCEPTED if:
        //The resource has been created (for POST/PUT requests only) but will not appear on
        //the website or be accessible via the API until it has been moderated.
        Accepted=202,
        //The API must return 304 NOT MODIFIED if:
        //The user has attempted to create a resource which already exists
        NotModified=304,
        //The API must return 400 BAD REQUEST under any one or more of the following
        //conditions:
        //Invalid request URI
        //Invalid HTTP Header
        //Receiving an unsupported parameter
        //Receiving a value for a parameter which fails validation
        //Receiving an invalid HTTP Message Body
        BadRequest=400,
        //If the user has not included an API key as a URL parameter on their request.
        //If the user is attempting to perform an operation upon a resource for which they do not
        //have the correct
        //permission (eg. attempting to update the user details of a different user)
        UnAuthorized=401,
        //The user is attempting to access a method on a resource which has not been created
        //The user is authenticated, but is attempting a PUT/POST/DELETE method on a
        //resource to which they
        //do not have permission
        Forbidden=403,
        //The server has not found a resource that matches the request URI.
        NotFound=404,
        //The method specified in the Request-Line is not allowed for the resource identified by
        //the Request-
        //URI. The response MUST include an Allow header containing a list of valid methods for
        //the requested
        //resource.
        MethodNotAllowed=405,
        //If there are errors in creating/updated/deleting a resource which are not covered by any
        //of the other
        //given status codes
        InternalServerError=500
    }
}
