Feature: Verify wording for acceptance on TOS screen
    @TestCaseKey=CLOUD-T49
    Scenario: Verify wording for acceptance on TOS screen
        
        Given the user is on the TOS screen
        
        When the user goes to accept the TOS
        
        Then the user sees "By signing up for a trial, you agree to our Terms of Service"