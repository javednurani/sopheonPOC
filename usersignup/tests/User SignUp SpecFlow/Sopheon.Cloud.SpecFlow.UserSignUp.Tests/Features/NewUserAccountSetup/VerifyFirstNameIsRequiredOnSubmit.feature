Feature: Verify First Name is Required on Submit
    @TestCaseKey=CLOUD-T34
    Scenario: Verify First Name is Required on Submit
        
        Given the user is on the PL Account Sign Up page
        
        When the user clicks submit with a blank First Name Field
        
        Then the user is not taken to new account page