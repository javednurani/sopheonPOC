Feature: Verify Password is Required on Submit
    @TestCaseKey=CLOUD-T44
    Scenario: Verify Password is Required on Submit
        
        Given the user is on the PL Account Sign Up page
        
        When the user clicks submit with a blank Password Field
        
        Then the user is not taken to new account page