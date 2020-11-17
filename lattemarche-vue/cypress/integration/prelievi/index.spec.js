const { isExportDeclaration, isGetAccessor } = require("typescript");

describe('Prelievi - List', () => {

    beforeEach(() => {    
      cy.login();
    })
  
    afterEach(() => {
       cy.logout();
    })

    // lista prelievi
    it('List', () => {    
    
        cy.visit(Cypress.env('url') + '/prelievi');    

        cy.get('table > tbody').find('tr').should('have.length', 1);
  
        cy.get('#cy-data-inizio').type('01-01-2020');
        cy.get('#cy-data-fine').type('01-01-2021');
        cy.get('#cy-lotto').type('TT2909201612');

        cy.get('#cy-btn-search').click();
  
        cy.get('table > tbody').find('tr').should('not.have.length', 1);
        
    });

    // gestione parametri hash
    it('Hash', () => {    
        
        cy.visit(Cypress.env('url') + '/prelievi');    

        cy.url().should('not.contains', 'IdDestinatario');

        cy.get('#cy-data-inizio').type('01-01-2020');
        cy.get('#cy-data-fine').type('01-01-2021');

        cy.get('#cy-lotto').type('1234');
        cy.select2_PickValue('#cy-destinatario');        

        cy.url().should('contains', 'IdDestinatario');

        cy.visit(Cypress.env('url'));    
        cy.go('back');

        cy.url().should('contains', 'IdDestinatario');

        cy.get('#cy-lotto').should('have.value', '1234');
        cy.get('table > tbody').find('tr').should('not.have.length', 1);

    });

    // annulla selezioni
    it('Clear', ()  => {

      cy.visit(Cypress.env('url') + '/prelievi');    

      cy.get('#cy-data-inizio').type('01-01-2020');
      cy.get('#cy-data-fine').type('01-01-2021');
      cy.get('#cy-lotto').type('1234');
      cy.select2_PickValue('#cy-giro');        
      cy.select2_PickValue('#cy-allevatore');        
      cy.select2_PickValue('#cy-trasportatore');        
      cy.select2_PickValue('#cy-tipo-latte');        
      cy.select2_PickValue('#cy-acquirente');        
      cy.select2_PickValue('#cy-cessionario');       
      cy.select2_PickValue('#cy-destinatario');         

      cy.get('#cy-btn-clear').click();

      cy.get('#cy-data-inizio').should('not.have.value', '01-01-2020');
      cy.get('#cy-data-fine').should('not.have.value', '01-01-2021');

      cy.get('#cy-lotto').should('have.value', '');
      cy.get('#cy-giro').should('have.value', '');

      cy.get('#cy-allevatore').should('have.value', '');
      cy.get('#cy-trasportatore').should('have.value', '');
      cy.get('#cy-tipo-latte').should('have.value', '');
      cy.get('#cy-acquirente').should('have.value', '');
      cy.get('#cy-cessionario').should('have.value', '');
      cy.get('#cy-destinatario').should('have.value', '');


    });
  
  })
  