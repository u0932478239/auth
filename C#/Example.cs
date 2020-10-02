		Auth.Password = "enc key"; //change this
		Auth.Login(username, userpass);
            if (Auth.Answer == true)
            {
                MessageBox.Show("Welcome Back, " + username, "Example", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //Code for when the login is correct
            }
            else if (Auth.Answer == false && Auth.Error == "Passowrd Incorrect")
            {
                MessageBox.Show("Invalid Password", "Example", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (Auth.Answer == false && Auth.Error == "User Does Not Exist")
            {
                MessageBox.Show("User Does Not Exist", "Example", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (Auth.Answer == false && Auth.Error == "Session Expired")
            {
                MessageBox.Show("Expired Session", "Example", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Unkown Error", "Example", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }