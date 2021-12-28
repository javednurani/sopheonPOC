// INFO: hard coded messages representing resourced language strings

export const messages: Record<string, Record<string, string>> = {
  en: {
    'auth.myprofile': 'My Profile',
    'auth.signout': 'Sign Out',
    'auth.loginbutton': 'Log In',
    'auth.changepassword': 'Change Password',
    'auth.logoutwarning': 'Are you still working? You will be logged out in {time, number} seconds.',
    'authlanding.loginspinner': 'Login',
    'authlanding.signupspinner': 'Sign Up',
    'header.welcome': 'Page Title',
    'header.useDarkTheme': 'Dark Theme',
    'acct.account': 'Account',
    'nav.accoladeAlt': 'Accolade logo',
    'nav.product_app': 'Product',
    'aria.increment': 'Increment',
    'aria.decrement': 'Decremement',
    'aria.submit': 'Submit',
    'closemodal': 'Close popup modal',
    'fieldisrequired': 'This field is required',
    'name': 'Name',
    'yes': 'Yes',
    'no': 'No',
    'edit': 'Edit',
    'share': 'Share',
    'user.signin': 'Sign In',
    'user.signout': 'Sign Out',
    'ok': 'Ok',
    'cancel': 'Cancel',
    'discard': 'Discard',
    'unsaveddata': 'Unsaved data will be lost.',
    'save': 'Save',
    'add': 'Add',
    'step1': 'Step 1',
    'step2': 'Step 2',
    'step3': 'Step 3',
    'next': 'next',
    'continue': 'Continue',
    'getStarted': 'Get Started!',
    'sopheon': 'Sopheon',
    'defaultTitle': 'Template Title',
    'error.erroroccurred': 'An Error Occurred',
    'error.componentmissing': 'Component could not be loaded.',
    'error.errorcomponentmissing': 'Error component could not be loaded.',
    'fallback.loading': 'Loading...',
    'industryoption.default': 'Select an option',
    'industryoption.advertising': 'Advertising',
    'industryoption.agricuture': 'Agriculture & Forestry',
    'industryoption.construction': 'Construction',
    'industryoption.eduhigher': 'Education - Higher Ed',
    'industryoption.eduk12': 'Education - K12',
    'industryoption.energy': 'Energy, Mining, Oil & Gas',
    'industryoption.financialservices': 'Financial Services',
    'industryoption.govfederal': 'Government - Federal',
    'industryoption.govlocal': 'Government - Local',
    'industryoption.govmilitary': 'Government - Military',
    'industryoption.govstate': 'Government - State',
    'industryoption.healthcare': 'Health Care',
    'industryoption.insurance': 'Insurance',
    'industryoption.manuaero': 'Manufacturing - Aerospace',
    'industryoption.manuauto': 'Manufacturing - Automotive',
    'industryoption.manuconsumergoods': 'Manufacturing - Consumer Goods',
    'industryoption.manuindustrial': 'Manufacturing - Industrial',
    'industryoption.entertainment': 'Media & Entertainment',
    'industryoption.membershiporg': 'Membership Organizations',
    'industryoption.nonprofit': 'Non-Profit',
    'industryoption.pharma': 'Pharmaceuticals & Biotech',
    'industryoption.protechservices': 'Professional & Technical Services',
    'industryoption.realestate': 'Real Estate, Rental & Leasing',
    'industryoption.retail': 'Retail',
    'industryoption.techhardware': 'Technology Hardware',
    'industryoption.techsoftware': 'Technology Software & Services',
    'industryoption.telecom': 'Telecommunications',
    'industryoption.transportation': 'Transportation & Warehousing',
    'industryoption.travel': 'Travel, Leisure & Hospitality',
    'industryoption.utilities': 'Utilities',
    'onboarding.setupproduct': 'Set up your Product',
    'onboarding.yourproductname': 'What is the name of your product?',
    'onboarding.industryselection': 'What industries does the product serve?',
    'onboarding.step2of3': 'Step 2 of 3: Set up your Product',
    'onboarding.nextGoals': 'Next: Set up your Goals',
    'onboarding.setupYourGoals': 'Set up your Goals',
    'onboarding.pleaseLogin': 'Please Log In to use the Product App.',
    'onboarding.productKpi': "What key performance indicators (KPI) measure or track your product's success?",
    'onboarding.getstarted': 'Get Started!',
    'onboarding.productgoal': 'What is your current product goal?',
    'onboarding.step3of3': 'Step 3 of 3: Set up your Goals',
    'onboarding.done': 'Done: Get Started!',
    'nav.planning_app': 'Planning',
    'auth.notifications': 'Notifications',
    'calendar.selectadate': 'Select a date',
    'calendar.jan': 'Jan',
    'calendar.feb': 'Feb',
    'calendar.mar': 'Mar',
    'calendar.apr': 'Apr',
    'calendar.may': 'May',
    'calendar.jun': 'Jun',
    'calendar.jul': 'Jul',
    'calendar.aug': 'Aug',
    'calendar.sep': 'Sep',
    'calendar.oct': 'Oct',
    'calendar.nov': 'Nov',
    'calendar.dec': 'Dec',
    'calendar.janlong': 'January',
    'calendar.feblong': 'February',
    'calendar.marlong': 'March',
    'calendar.aprlong': 'April',
    'calendar.maylong': 'May',
    'calendar.junlong': 'June',
    'calendar.jullong': 'July',
    'calendar.auglong': 'August',
    'calendar.seplong': 'September',
    'calendar.octlong': 'October',
    'calendar.novlong': 'November',
    'calendar.declong': 'December',
    'calendar.sun': 'S',
    'calendar.mon': 'M',
    'calendar.tue': 'T',
    'calendar.wed': 'W',
    'calendar.thu': 'T',
    'calendar.fri': 'F',
    'calendar.sat': 'S',
    'calendar.sunlong': 'Sunday',
    'calendar.monlong': 'Monday',
    'calendar.tuelong': 'Tuesday',
    'calendar.wedlong': 'Wednesday',
    'calendar.thulong': 'Thursday',
    'calendar.frilong': 'Friday',
    'calendar.satlong': 'Saturday',
    'calendar.gototoday': 'Go to today',
    'calendar.gotoprevmonth': 'Go to previous month',
    'calendar.gotonextmonth': 'Go to next month',
    'calendar.gotoprevyear': 'Go to previous year',
    'calendar.gotonextyear': 'Go to next year',
    'calendar.closedatepicker': 'Close date picker',
    'calendar.selecttochangeyear': '{0}, select to change the year',
    'calendar.selecttochangemonth': '{0}, select to change the month',
    'toDo.title': 'To Do',
    'toDo.empty1': "You don't have any tasks yet. Click ",
    'toDo.empty2': ' above to add one.',
    'toDo.newtask': 'New Task',
    'toDo.tasknameplaceholder': 'Name your task',
    'toDo.tasknotesplaceholder': 'Add any details about the task',
    'toDo.notes': 'Notes',
    'toDo.duedate': 'Due Date',
    'toDo.selectastatus': 'Select a status',
    'toDo.discardthistask': 'Discard this task?',
    'status': 'Status',
    'status.notstarted': 'Not Started',
    'status.inprogress': 'In Progress',
    'status.assigned': 'Assigned',
    'status.complete': 'Complete',
    'sidebar.dashboard': 'Dashboard',
  },
};
