describe('Utenti - List', () => {

    beforeEach(() => {    
      cy.login();
    })
  
    afterEach(() => {
       cy.logout();
    })

    // lista utenti
    it('List', () => {    
    
        cy.visit(Cypress.env('url') + '/utenti');    
        
    });

    // gestione parametri hash
    it('Hash', () => {    
        
        cy.visit(Cypress.env('url') + '/utenti');    

        cy.select2_PickValue('#cy-profilo');        

        cy.url().should('contains', 'IdProfilo');

        cy.visit(Cypress.env('url'));    
        cy.go('back');

        cy.url().should('contains', 'IdProfilo');

        cy.get('#cy-profilo').should('have.value', '7');

    });

  
  })
  