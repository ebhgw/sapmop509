/*
 * Copyright 2010 Hogwart
 *
 * Security, User, Group function
 *
 */

 function Security () {
 }

// this seems to work only client side, user.perform
 Security.createUser = function () {
 // Login with standard admin user rights
 var session = formula.login( 'localhost', 8080, 'http', "admin", "formula", 60 )
 var users = formula.Root.findElement( 'users=Users/security=Security/root=Administration' );
 users.perform( session, 'LifeCycle|Create', [], [
                 'testuser', // User name
                 'password', // Password
                 'name surname', // Full name
                 '', // email
                 '', // phone
                 '', // fax
                 '', // pager
                 'users' // Group membership (comma-delimited list)
 ] )
 }

  Security.createUserServerSide = function () {
  // Login with standard admin user rights

  var users = formula.Root.findElement( 'users=Users/security=Security/root=Administration' );
  users.perform( session.getReference(), 'LifeCycle|Create', [], [
                  'testuser', // User name
                  'password', // Password
                  'name surname', // Full name
                  '', // email
                  '', // phone
                  '', // fax
                  '', // pager
                  'users' // Group membership (comma-delimited list)
  ] )
  }