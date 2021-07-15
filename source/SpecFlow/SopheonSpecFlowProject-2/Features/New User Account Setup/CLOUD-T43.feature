Feature: Verify E-mail Address is Required on Submit
    @TestCaseKey=CLOUD-T43
    Scenario: Verify E-mail Address is Required on Submit
        
        Given the user is on the PL Account Sign Up page
        
        When the user clicks submit with a blank E-mail Address Field
        
        Then the user is not taken to new account page