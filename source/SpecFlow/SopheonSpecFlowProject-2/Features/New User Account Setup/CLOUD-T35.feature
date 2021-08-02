Feature: Verify Password Hint is displayed to user
    @TestCaseKey=CLOUD-T35
    Scenario: Verify Password Hint is displayed to user
        
        Given the user is on the PL Account Sign Up page
        
        When the user clicks Submit with a password not meeting default settings (all lower case)
        
        Then the user is given a Password hint message 