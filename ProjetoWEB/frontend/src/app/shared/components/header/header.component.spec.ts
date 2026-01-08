import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { HeaderComponent } from './header.component';

describe('HeaderComponent', () => {
  let component: HeaderComponent;
  let fixture: ComponentFixture<HeaderComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HeaderComponent, FormsModule, RouterModule.forRoot([])]
    }).compileComponents();

    fixture = TestBed.createComponent(HeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('Component initialization', () => {
    it('should initialize with empty search query', () => {
      expect(component.searchQuery).toBe('');
    });

    it('should initialize with zero cart items', () => {
      expect(component.cartItemCount).toBe(0);
    });
  });

  describe('Search functionality', () => {
    it('should emit search event with trimmed query on enter', () => {
      const searchSpy = jest.spyOn(component.search, 'emit');
      component.searchQuery = '  test query  ';

      component.onSearch();

      expect(searchSpy).toHaveBeenCalledWith('test query');
    });

    it('should not emit search event when query is empty', () => {
      const searchSpy = jest.spyOn(component.search, 'emit');
      component.searchQuery = '   ';

      component.onSearch();

      expect(searchSpy).not.toHaveBeenCalled();
    });

    it('should not emit search event when query is empty string', () => {
      const searchSpy = jest.spyOn(component.search, 'emit');
      component.searchQuery = '';

      component.onSearch();

      expect(searchSpy).not.toHaveBeenCalled();
    });
  });

  describe('Cart functionality', () => {
    it('should emit cartClick event when cart button is clicked', () => {
      const cartClickSpy = jest.spyOn(component.cartClick, 'emit');

      component.onCartClick();

      expect(cartClickSpy).toHaveBeenCalled();
    });

    it('should display cart badge when cart has items', () => {
      component.cartItemCount = 5;
      fixture.detectChanges();

      const badge = fixture.nativeElement.querySelector('.cart-badge');
      expect(badge).toBeTruthy();
      expect(badge.textContent.trim()).toBe('5');
    });

    it('should not display cart badge when cart is empty', () => {
      component.cartItemCount = 0;
      fixture.detectChanges();

      const badge = fixture.nativeElement.querySelector('.cart-badge');
      expect(badge).toBeFalsy();
    });
  });

  describe('Profile functionality', () => {
    it('should emit profileClick event when profile button is clicked', () => {
      const profileClickSpy = jest.spyOn(component.profileClick, 'emit');

      component.onProfileClick();

      expect(profileClickSpy).toHaveBeenCalled();
    });
  });

  describe('Template rendering', () => {
    it('should render the logo', () => {
      const logo = fixture.nativeElement.querySelector('.logo');
      expect(logo).toBeTruthy();
    });

    it('should render navigation links', () => {
      const navLinks = fixture.nativeElement.querySelectorAll('.nav-link');
      expect(navLinks.length).toBe(3);
    });

    it('should render search input', () => {
      const searchInput = fixture.nativeElement.querySelector('.search-input');
      expect(searchInput).toBeTruthy();
      expect(searchInput.placeholder).toBe('Buscar produtos, lojas...');
    });

    it('should render cart button', () => {
      const cartButton = fixture.nativeElement.querySelector('.action-btn[aria-label="Carrinho de compras"]');
      expect(cartButton).toBeTruthy();
    });

    it('should render profile button', () => {
      const profileButton = fixture.nativeElement.querySelector('.action-btn[aria-label="Perfil do usuário"]');
      expect(profileButton).toBeTruthy();
    });
  });

  describe('User interactions', () => {
    it('should update searchQuery when user types in search input', () => {
      const searchInput = fixture.nativeElement.querySelector('.search-input');
      searchInput.value = 'new search';
      searchInput.dispatchEvent(new Event('input'));
      fixture.detectChanges();

      expect(component.searchQuery).toBe('new search');
    });

    it('should call onSearch when Enter key is pressed in search input', () => {
      const onSearchSpy = jest.spyOn(component, 'onSearch');
      const searchInput = fixture.nativeElement.querySelector('.search-input');

      const event = new KeyboardEvent('keyup', { key: 'Enter' });
      searchInput.dispatchEvent(event);
      fixture.detectChanges();

      expect(onSearchSpy).toHaveBeenCalled();
    });

    it('should call onCartClick when cart button is clicked', () => {
      const onCartClickSpy = jest.spyOn(component, 'onCartClick');
      const cartButton = fixture.nativeElement.querySelector('.action-btn[aria-label="Carrinho de compras"]');

      cartButton.click();
      fixture.detectChanges();

      expect(onCartClickSpy).toHaveBeenCalled();
    });

    it('should call onProfileClick when profile button is clicked', () => {
      const onProfileClickSpy = jest.spyOn(component, 'onProfileClick');
      const profileButton = fixture.nativeElement.querySelector('.action-btn[aria-label="Perfil do usuário"]');

      profileButton.click();
      fixture.detectChanges();

      expect(onProfileClickSpy).toHaveBeenCalled();
    });
  });
});
