Authentication: verify you are who you say you are and generate the security context.

Security context: all you identity info that is relevant to the facility.
    - Contains all the information the user has (user name, addresses, etc).
    - Encapsulated into one single object: claims principal. Object that represents the security context of the user (it is the user).
    - It can have one or many identities.
    - One identity can have many claims. Claims are a key value pair that carry the users information.

Authorization: Verifying the security context satisfies the access requirements.

The server verify the user's credentials and returns the identity (security context) to the browser.