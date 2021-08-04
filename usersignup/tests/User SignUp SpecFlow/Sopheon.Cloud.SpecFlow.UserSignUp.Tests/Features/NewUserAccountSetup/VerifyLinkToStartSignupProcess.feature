Feature: Verify Link to start signup process
    @TestCaseKey=CLOUD-T32
    Scenario: Verify Link to start signup process
        
        Given The user is on the landing page (Test endpoint URL tbd)
        
        When the user clicking on the Sign Up link 
        
        Then the user is taken to the PL Account Setup Page (redirect to new page)